<template>
  <div style='display: inline-block;'>
    <b-button @click='editMode = false; clearForm();' v-b-modal.modal-1>Click here to leave a review.</b-button>

    <b-modal id="modal-1" title="Write a Review:">
      <b-form>
        <b-form-group id="title-input-group" label="Title: " label-for="title-input">
          <b-form-input id="title-input" v-model="review.title" type="text" required></b-form-input>
        </b-form-group>
        <b-form-group id="detail-input-group" label="Details: " label-for="detail-input">
          <b-form-input id="detail-input" v-model="review.detail" type="text" required></b-form-input>
        </b-form-group>
        <b-form-group id="rating-input-group" label="Rating: " label-for="rating-input">
          <star-rating v-bind:show-rating='false' v-model="review.rating"></star-rating>
        </b-form-group>
      </b-form>
      <template v-slot:modal-footer="{ ok, cancel }">
        <b-button size="sm" variant="success" @click="onSubmit()">Submit</b-button>
        <b-button size="sm" variant="danger" @click="cancel()">Cancel</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
import StarRating from "vue-star-rating";
import auth from "../../auth";
export default {
  props: ["typeId", "apiPath"],
  data() {
    return {
      review: {
        title: "",
        detail: "",
        rating: 0,
        typeId: this.typeId,
        userId: auth.getUser().id,
        editMode: false
      }
    };
  },
  components: {
    StarRating
  },
  methods: {
    onSubmit() {
      if(this.editMode)
      {
        fetch(process.env.VUE_APP_REMOTE_API + "/api/" + this.apiPath + this.review.id, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${auth.getToken()}`
          },
          body: JSON.stringify(this.review)
        }).catch(err => {
          console.log(err);
          this.alertTime = 5;
        });
        this.$emit('review-edited', {
            title: this.review.title, 
            userId: this.review.userId,
            rating: this.review.rating,
            detail: this.review.detail,
            id: this.review.id
        });
        this.clearForm();
        this.$bvModal.hide('modal-1')
      }
      else
      {
        fetch(process.env.VUE_APP_REMOTE_API + "/api/" + this.apiPath, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${auth.getToken()}`
          },
          body: JSON.stringify(this.review)
        }).catch(err => {
          console.log(err);
          this.alertTime = 5;
        });
        this.$emit('review-submitted', {
            title: this.review.title, 
            userId: this.review.userId,
            rating: this.review.rating,
            detail: this.review.detail,
            upvoteCount: 0,
            downvoteCount: 0
        });
        this.clearForm();
        this.$bvModal.hide('modal-1')
      }
      
    },
    clearForm() {
      this.review.title = "";
      this.review.detail = "";
      this.review.rating = 0;
    },
    EditReview(review)
    {
      this.review.title = review.title;
      this.review.detail = review.detail;
      this.review.rating = review.rating;
      this.review.id = review.id;
      this.$bvModal.show('modal-1');
      this.editMode = true;
    }
  }
};
</script>