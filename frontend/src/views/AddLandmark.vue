<template>
  <div>
    <b-container >
    <b-alert
      :show="isSuccess"
      variant="success"
      style="text-align: center"
    ><span v-if='isAdmin'>Landmark succesfully added.</span><span v-else>Your landmark has been submitted for review.</span></b-alert>
    <b-alert
      :show="alertTime"
      @dismissed="alertTime = 0"
      variant="danger"
      style="text-align: center"
    >Landmark not succesfully added.</b-alert></b-container>
  <b-container id="addContainer">
    
    <b-form @submit="writeLandmarks" @reset="onReset" v-if="show" id="addForm">
      <br />
      <b-form-group id="input-group-1" label="Long Name: " label-for="input-1">
        <b-form-input id="input-1" v-model="landmark.longName" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-2" label="Short Name: " label-for="input-2">
        <b-form-input id="input-2" v-model="landmark.shortName" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-3" label="Description: " label-for="input-3">
        <b-form-input id="input-3" v-model="landmark.details" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-4" label="Street Number: " label-for="input-4">
        <b-form-input id="input-4" v-model="landmark.streetNumber" type="number" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-5" label="Street Name: " label-for="input-5">
        <b-form-input id="input-5" v-model="landmark.streetName" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-6" label="City: " label-for="input-6">
        <b-form-input id="input-6" v-model="landmark.city" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-7" label="State: " label-for="input-7">
        <b-form-input id="input-7" v-model="landmark.state" type="text" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-8" label="State Code: " label-for="input-8">
        <b-form-input id="input-8" v-model="landmark.stateCode" type="text" :maxlength="2" required></b-form-input>
      </b-form-group>
      <b-form-group id="input-group-9" label="Zip: " label-for="input-9">
        <b-form-input id="input-9" v-model="landmark.zip" type="number" required></b-form-input>
      </b-form-group>
      <b-button type="submit" variant="primary">Submit</b-button>
      <b-button type="reset" variant="danger">Reset</b-button>
    </b-form>
    <br />
    
    <br />
    <gmap-map
      :center="center"
      @dismissed="isSuccess = false"
      :zoom="12"
      :options='$root.mapOptions'
      style="width:50%;  height: auto; margin-left: 100px"
      @rightclick="clicked"
    >
      <gmap-marker
        :key="index"
        v-for="(m, index) in markers"
        :position="m.position"
        @click="center=m.position"
        :draggable="true"
      ></gmap-marker>
      <GmapInfoWindow />
    </gmap-map>
  </b-container>
  </div>
</template>


<script>
import auth from "../auth";
export default {
  data() {
    return {
      landmark: {
        longName: "",
        shortName: "",
        latitude: 0,
        longitude: 0,
        city: "",
        details: "",
        streetNumber: 0,
        zip: 0,
        streetName: "",
        state: "",
        stateCode: "",
        id: 0,
        isApproved: false
      },
      show: true,
      center: { lat: 45.508, lng: -73.587 },
      markers: [],
      places: [],
      currentPlace: null,
      isSuccess: false,
      alertTime: 0,
      isAdmin: false
    };
  },
  methods: {
    onReset(evt, isSuccess) {
      evt.preventDefault();
      // Reset our form values
      this.landmark.shortName = "";
      this.landmark.longName = "";
      this.landmark.details = "";
      this.landmark.latitude = 0;
      this.landmark.longitude = 0;
      this.landmark.city = "";
      this.landmark.details = "";
      this.landmark.streetNumber = 0;
      this.landmark.zip = 0;
      this.landmark.streetName = "";
      this.landmark.state = "";
      if (isSuccess) {
        isSuccess = true;
      }
      // Trick to reset/clear native browser form validation state
      this.show = false;
      this.$nextTick(() => {
        this.show = true;
      });
    },
    writeLandmarks(evt) {
      evt.preventDefault();
      if (this.isAdmin) {
        fetch(process.env.VUE_APP_REMOTE_API + "/api/landmark/create", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${auth.getToken()}`
          },
          body: JSON.stringify(this.landmark)
        })
          .then(response => {
            if (response.ok) {
              this.isSuccess = true;
              this.onReset(evt, this.isSuccess);
            }
          })
          .catch(err => {
            console.log(err);
            this.alertTime = 5;
          });
      } else {
        fetch(process.env.VUE_APP_REMOTE_API + "/api/landmark/suggest", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${auth.getToken()}`
          },
          body: JSON.stringify(this.landmark)
        })
          .then(response => {
            if (response.ok) {
              this.isSuccess = true;
              this.onReset(evt, this.isSuccess);
            }
          })
          .catch(err => {
            console.log(err);
            this.alertTime = 5;
          });
      }
    },

    geocodeLandmark(marker) {
      this.$gmapApiPromiseLazy().then(gmap => {
        let geocoder = new gmap.maps.Geocoder();
        let latLng = { lat: marker.lat, lng: marker.lng };
        geocoder.geocode({ location: latLng }, (results, status) => {
          if (status === "OK") {
            const location = results[0].address_components;
            console.log(location);
            this.landmark.latitude = marker.lat;
            this.landmark.longitude = marker.lng;
            this.landmark.city = location[3].long_name;
            this.landmark.streetNumber = location[0].long_name;
            this.landmark.zip = location[7].long_name;
            this.landmark.streetName = location[1].long_name;
            this.landmark.state = location[5].long_name;
            this.landmark.stateCode = location[5].short_name;
            this.landmark.isApproved = this.isAdmin;
          }
        });
      });
    },
    clicked(e) {
      this.currentPlace = {
        latLng: e.latLng
      };
      if (this.markers.length === 1) {
        this.markers.pop();
      }
      if (this.currentPlace) {
        const marker = {
          lat: this.currentPlace.latLng.lat(),
          lng: this.currentPlace.latLng.lng()
        };
        this.geocodeLandmark(marker);
        this.markers.push({ position: marker });
        this.places.push(this.currentPlace);
        this.center = marker;
        this.currentPlace = null;
      }
    },
    geolocate: function() {
      navigator.geolocation.getCurrentPosition(position => {
        this.center = {
          lat: position.coords.latitude,
          lng: position.coords.longitude
        };
      });
    }
  },
  created()
  {
    if(auth.getUser() != null)
    {
       if(auth.getUser().rol == 'admin')
          {
            this.admin = true;
          }
    }
    this.$bus.$on('login', () => {
        if(auth.getToken() != null)
        {
          if(auth.getUser().rol == 'admin')
          {
            this.admin = true;
          }
        }
    })
  },
  mounted() {
    this.geolocate();
    this.isAdmin = auth.getUser().rol === "admin";
  }
};
</script>

<style scoped>
  #addContainer {
    margin-top: 50px;
    margin-bottom: 100px;
    display: flex;
  }

  #addForm {
    width: 50%;
  }

</style>