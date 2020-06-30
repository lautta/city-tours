<template>
    <div>
        <LoadingIcon center v-if='landmark===null'></LoadingIcon>
        <div v-else>
            <b-container id='detailContainer'>
                <b-row><h1 id='landmarkName'>{{landmark.longName}}</h1></b-row>
                <b-row>
                    <b-col style='padding-left: 0px;'>
                        
                        <b-carousel
                            v-if="landmark != null && landmark.imageNames.length > 0"
                            class="landmarkImage"
                            style="text-shadow: 0px 0px 2px #000"
                            fade
                            :interval='3400'
                            img-width="600"
                            img-height="250"
                        >
                            <b-carousel-slide
                            class='landmarkCarousel'
                            v-for='(imageName, index) in landmark.imageNames'
                            :key='index'
                            caption=""
                            img-height="250"
                            img-rounded
                            :img-src="'/uploads/' + id + '/' + imageName"
                            >
                            </b-carousel-slide>
                        </b-carousel>
                        <b-img v-else src='@/assets/default.jpg' class='landmarkImage'></b-img>
                    </b-col>
                    <b-col style='padding-right: 0px;'>
                        <GmapMap id='googleMap'
                        :center="{lat:landmark.latitude, lng:landmark.longitude}"
                        :zoom="12"
                        :options="$root.mapOptions"
                        map-type-id="terrain"
                        style="width: 600px; height: 250px"
                        >   
                        </GmapMap>
                        <b-row id='address'><b>{{landmark.streetNumber}} {{landmark.streetName}} {{landmark.city}} {{landmark.zip}}</b>&nbsp;&nbsp;&nbsp;</b-row>
                    </b-col>
                </b-row>
            
                <b-row id='details'>{{landmark.details}}</b-row>
                <br>
                <b-row v-if="signedIn">
                    <WriteReview ref='editor' @review-edited='ReviewEdited' @review-submitted='ReviewSubmitted'  :typeId=this.id apiPath='review/landmark/'></WriteReview>
                    <b-form class='ml-auto' ref='imageForm' :action="GetImageUrl" method='post'>                 
                        <b-form-file
                            
                            v-if='!imageUploaded'
                            v-model="imageUpload"
                            :state="Boolean(imageUpload)"
                            placeholder="Upload an image of this location"
                            drop-placeholder="Drop image here..."
                            style='width: 400px; float: right;'
                            accept="image/*"
                            enctype="form-data"
                            v-on:input='AttemptFileUpload'
                        ></b-form-file>
                        <span v-else style='float: right;'>Your image has been uploaded</span>
                    </b-form>
                </b-row>
            </b-container>

        </div>
        <br>
        <Reviews ref='reviews' apiPath="review/landmark/"></Reviews>
        <br><br>
    </div>
</template>

<script>
import WriteReview from './components/WriteReview'
import Reviews from './components/Review.vue';
import auth from '../auth'
export default {
  name: 'landmark',
  components: {
      Reviews,
      WriteReview
  },
  data() {
    return {
        imageUpload: null,
        id: this.$route.params.id,
        landmark: null,
        signedIn: auth.getUser() != null,
        imageUploaded: false
    };
  },
  methods: {
      getImage()
      {
          try {
            return require('@/assets/' + this.id + '.jpg');
          }
          catch{ 
              return require('@/assets/default.jpg');
          }
      },
      getLandmarkDetails(id)
      {
            fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/` + id, {
                method: 'GET',
                headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
                }
            })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                } else {
                    this.landmarkNotFound();
                }
            })
            .then((landmark) => {
                if(landmark.hasOwnProperty("longName") && landmark.longName != null)
                {
                    this.landmark = landmark;
                }
                else{
                    this.landmarkNotFound();
                }
            })
            .catch((err) => {
                console.log(err);
                this.landmarkNotFound()
            });
        },
        landmarkNotFound()
        {
            this.$router.push({name: 'not-found'});
        },
        ReviewSubmitted(review){
            this.$refs.reviews.AddReview(review);
        },
        ReviewEdited(review){
            this.$refs.reviews.EditReview(review);
        },
        AttemptFileUpload(file)
        {
            //this.$refs.imageForm.submit();
            let formData = new FormData();
            formData.append('File', file);
            formData.append('Name', file.name);
            
            this.imageUploaded = true;
            fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/` + this.id + "/upload", {
                method: 'POST',
                headers: {
                    Accept: 'application/json',
                    Authorization: `Bearer ${auth.getToken()}`
                },
                body: formData
            })
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
            })
            .then((result) => {
                this.landmark.imageNames.push(result.message);
            })
            .catch((err) => {
                console.log(err);
            });
        }
  },
  created() {
      this.getLandmarkDetails(this.id);
      
      this.$bus.$on('login', () => {
        if(auth.getToken() != null)
        {
          this.signedIn = true;
        }
        })
  },
  computed:
  {
      GetImageUrl()
      {
          return `${process.env.VUE_APP_REMOTE_API}/api/landmark/` + this.id + '/upload'
      }
  }
}

</script>

<style scoped>
    .landmarkImage{
        width: 400px;
        height: 250px;
        text-align: center;
        line-height: 250px;
        
    }
    #detailContainer{
        margin-top: 100px;
    }
    #landmarkName {
        margin-bottom: 34px;
    }
    #details{
        margin-top: 33px;
    }
    #googleMap{
        float: right;
        margin-left: 100%;
    }
    #address{
        float: right;
    }
    
</style>
