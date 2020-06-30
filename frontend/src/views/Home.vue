<template>
  <b-container id="homeContainer">
    <h1>City Tours</h1>
    <h3>Plan your next city tour!</h3>
    <div>
      <br>
    <b-carousel
      id="carousel-fade"
      style="text-shadow: 0px 0px 2px #000;"
      fade
      indicators
      img-width="1024"
      img-height="480"
    >
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/Columbus-Ohio.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/Boston-MA.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/day-to-night-NY.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/london.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/New-York-City.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/rickenbacker-causeway.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/seattle.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/somwhere.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/toronto.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/toronto-canada.jpg"
      ></b-carousel-slide>
      <b-carousel-slide
        caption=""
        img-src="@/assets/skylines/who-knows-where.jpg"
      ></b-carousel-slide>
    </b-carousel>
  </div>
    <b-container id="itineraryContainer">
    <h4>Here are the available itineraries</h4>
    <b-table v-if="!unabletoLoad" class="itinerary-list" @row-clicked='onTableClick' :items="this.itineraries" :fields="displayedFields" striped hover>
    </b-table>
    <p v-if="unabletoLoad">Can't load itineraries.</p>
  </b-container>
  </b-container>
</template>

<script>
export default {
  name: "home",
  data() {
    return {
      displayedFields: ['name', 'startLat', 'startLng' ],
      itineraries: [],
      unabletoLoad: false
    };
  },
  created() {
    this.getItineraries();
  },
  methods: {
    getItineraries() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/api/itinerary/`, {
        method: "GET",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        }
      })
        .then(response => {
          if (response.ok) {
            return response.json();
          } else {
            this.itinerariesNotFound();
          }
        })
        .then(itineraryList => {
          if (itineraryList.length > 0) {
            this.itineraries = itineraryList;
          }
          else{
            this.itinerariesNotFound();
          }
        })
        .catch(err => {
          console.log(err);
          this.itinerariesNotFound();
        });
    },
    itinerariesNotFound() {
      this.unabletoLoad = true;
    },
    onTableClick(e){
      this.$router.push({path: `/viewitinerary/${e.id}`})
    }
  }
};
</script>

<style scoped>
  #homeContainer {
    margin-top: 50px;
    text-align: center;
  }

  #itineraryContainer {
    padding: 50px;
  }

      .itinerary-list{
        cursor: pointer;
    }
 .carousel-item {
    height: 500px !important;
    width: auto !important;
    left: 50%;
    transform: translate(-50%, 0%) !important;
  }

</style>