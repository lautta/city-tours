<template>
    <b-container>
         <b-list-group>
            <b-list-group-item v-for='(review, index) in reviews' :key="index">
                
                <span><StarRating v-bind:inline='true' read-only v-bind:rating='review.rating' v-bind:show-rating='false' v-bind:star-size='30'></StarRating></span>
                
                <span style='margin-left: 10px; font-size: 22px;'><b>{{review.title}}</b></span>
                
                <b-button @click='AttemptEditReview(review)' style='float: right;' v-if='review.userId == userID'>Edit Review</b-button>
                <span v-else-if='!review.hasVoted' class='helpfulSpan'>Was this review helpful? 
                    <font-awesome-icon @click='thumbsUp(index)' class='helpfulButton' icon="thumbs-up" /> 
                    <font-awesome-icon @click='thumbsDown(index)' class='helpfulButton' icon="thumbs-down" />
                </span>
                <br>
                <span class='reviewInfo'>Review by {{review.userName}}. {{review.upvoteCount}}/{{review.downvoteCount + review.upvoteCount}} people found this review helpful</span>
                <br>
                {{review.detail}}
            </b-list-group-item>
        </b-list-group>
    </b-container>
</template>

<script>
import auth from '../../auth';
import StarRating from 'vue-star-rating';
export default {
    data() {
        return {
            id: this.$route.params.id,
            reviews: [],
            userID: -1
        };
    },
    props: [
        'apiPath',
        'reviewEditor'
    ],
    components: {
        StarRating
    },
    created() {
        if(auth.getUser() !== null)
        {
            this.userID = auth.getUser().id;
        }
        fetch(`${process.env.VUE_APP_REMOTE_API}/api/` + this.apiPath + "all/" + this.id, {
            method: 'GET',
            headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
            }
        })
        .then((response) => {
            if (response.ok) {
                return response.json();
            }
        })
        .then((reviews) => {
            this.reviews = [...reviews];
        })
        .catch((err) => {
            console.log(err);
        });
    },
    methods: {
        thumbsUp(index)
        {
            this.hideThumbs(index);
            fetch(`${process.env.VUE_APP_REMOTE_API}/api/` + this.apiPath + "upvote", {
                method: 'POST',
                headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                },
                body: this.reviews[index].id
            })
            .catch((err) => {
                console.log(err);
            });
            this.reviews[index].upvoteCount++;
            this.$forceUpdate();
        },
        thumbsDown(index)
        {
            this.hideThumbs(index);
            fetch(`${process.env.VUE_APP_REMOTE_API}/api/` + this.apiPath + "downvote", {
                method: 'POST',
                headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                },
                body: this.reviews[index].id
            })
            .catch((err) => {
                console.log(err);
            });
            this.reviews[index].downvoteCount++;
            this.$forceUpdate();
        },
        hideThumbs(index){
            this.reviews[index].hasVoted = true;
            this.$forceUpdate();
        },
        AddReview(review)
        {
            this.reviews.push(review);
        },
        AttemptEditReview(review)
        {
            this.$parent.$refs.editor.EditReview(review);
        },
        EditReview(review)
        {
            for(let i = 0; i < this.reviews.length; i++)
            {
                if(this.reviews[i].id == review.id)
                {
                    this.reviews[i].title = review.title;
                    this.reviews[i].detail = review.detail;
                    this.reviews[i].rating = review.rating;
                    break;
                }
            }
        }
    }
}
</script>

<style scoped>
    .helpfulSpan{
        float: right;
        color:dimgrey;
    }
    .helpfulButton{
        margin: 4px;
        cursor: pointer;
    }
    .reviewInfo{
        color: dimgrey;
        font-size: 12px;
    }
</style>