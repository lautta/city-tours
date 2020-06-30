<template>
    <b-container id="searchContainer">
        <LoadingIcon center v-if='this.displayedLandmarks.length == 0 && !unableToLoad && !zeroResults' ></LoadingIcon>
        <p style='text-align: center' v-if='unableToLoad'>The landmarks could not be loaded, please try again later</p>
        <div v-if='displayedLandmarks.length > 0 || zeroResults'>
            <span>Your Starting Location: </span><span>Latitude: {{location.lat}}</span><span>Longitude: {{location.lng}}</span>
            <p v-if="zeroResults">There are no landmarks close enough to your location, please adjust the maximum distance and try again.</p>
            <b-form id='searchForm' inline>
                <label for="location">Search For Your Starting Location:</label>
                <b-input v-model.lazy='locationSearch' @change='HelperRequestMap()'
                id="location"
                class="mb-2 mr-sm-2 mb-sm-0"
                placeholder=""
                ></b-input>

                <label for="distance">Maximum Distance From You (Miles)</label>
                <b-input-group class="mb-2 mr-sm-2 mb-sm-0">
                <b-input id="distance" v-model.lazy='maxDistance' @change='maxDistanceChange' placeholder="100"></b-input>
                </b-input-group>
            </b-form>
            <span v-if='searchError == true' style='color: red;'>The location you searched could not be found</span>
            <br>
            <b-table  class='tableLandmark' @row-clicked='onTableClick' striped hover :items="displayedLandmarks" :fields='displayedFields'>
                <template v-slot:cell(AddToItinerary)="row">
                    <b-button size="sm" @click="onAddClick(row)" class="mr-1">
                         Add
                    </b-button>
                </template>
            </b-table>
        </div>
         
    </b-container>
</template>

<script>
import mapsHelper from '../mapsHelper';
export default {
    name: "SearchLandmarks",
    props: [
        'itinerary',
        "coordinates"
    ],
    data() {
        return {
            landmarks: null,
            location: {lat: 43, lng: -90},
            unableToLoad: false,
            displayedLandmarks: [],
            locationSearch: "",
            searchError: false,
            zeroResults: false,
            maxDistance: 100,
            displayedFields: [
                {
                    key: "name",
                    sortable: true
                },
                {
                    key: "city",
                    sortable: true
                },
                {
                    key: "driving_distance",
                    sortable: true
                },
                {
                    key: "direct_distance",
                    sortable: true
                },
                {
                    key: "travel_time",
                    sortable: false
                },
                
            ]
        };
    },
    watch: {
        'coordinates' : function(){
            this.location.lat = this.coordinates.lat;
            this.location.lng = this.coordinates.lng;
        }
    },
    created(){
        this.getAllLandmarks();
        if("geolocation" in navigator && (this.coordinates === null || this.coordinates === undefined)) {
            navigator.geolocation.getCurrentPosition(pos => {
                
                if(this.coordinates == null || this.coordinates == undefined)
                {
                    this.location.lat = pos.coords.latitude;
                    this.location.lng = pos.coords.longitude;
                    this.$emit('locationChanged', this.location);
                }
                
            });
        }
        if(this.itinerary)
        {
            this.displayedFields.push({
                    key: "AddToItinerary",
                    sortable: false
                });
        }
    },
    methods: {
        getAllLandmarks()
        {
            fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/approved`, {
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
                else{
                    this.landmarksNotFound();
                }
            })
            .then((landmarkList) => {
                if(landmarkList.length > 0)
                {
                    this.landmarks = landmarkList;
                    this.populateDistances();
                    
                }
                else{
                    this.landmarksNotFound();
                }
            })
            .catch((err) => {
                console.log(err);
                this.landmarksNotFound()
            });
        },
        landmarksNotFound(){
            this.unableToLoad = true;
        },
        computeDisplayedLandmarks(){
            if(this.landmarks != null)
            {
                this.displayedLandmarks = [];
                for(let i = 0; i < this.landmarks.length; i++)
                {
                    let landmark = {};
                    landmark.location = {lat: this.landmarks[i].latitude, lng: this.landmarks[i].longitude};
                    landmark.name = this.landmarks[i].longName;
                    landmark.city = this.landmarks[i].city;
                    landmark.direct_distance = this.landmarks[i].directDistance;
                    landmark.driving_distance = this.landmarks[i].drivingDistance;
                    landmark.id = this.landmarks[i].id;
                    landmark.travel_time = this.landmarks[i].drivingTime;
                    if(typeof landmark.direct_distance != "undefined")
                    {
                        if(this.maxDistance > parseInt(landmark.direct_distance.substring(0, landmark.direct_distance.length - 3)) )
                        {
                            this.displayedLandmarks.push(landmark);
                            this.zeroResults = false;
                        }
                    }
                }
                if(this.landmarks.length > 0 && this.displayedLandmarks.length === 0)
                {
                    this.zeroResults = true;
                }
            }
        },
        searchChange(){
            this.$gmapApiPromiseLazy().then(gmap => {
                let geocoder = new gmap.maps.Geocoder();
                geocoder.geocode({'address': this.locationSearch}, (results, status) => {
                    if (status === 'OK') {
                        this.searchError = false;
                        this.location.lat = results[0].geometry.location.lat();
                        this.location.lng = results[0].geometry.location.lng();
                        this.$emit('locationChanged', this.location);
                        this.populateDistances();
                    }
                    else
                    {
                        this.searchError = true;
                    } 
                });
            });
        },
        populateDistances()
        {
            
            for(let i = 0; i < this.landmarks.length; i++)
            {
                mapsHelper.RequestMap(this.populateDistance, i);
                
            }
            
        },
        populateDistance(i)
        {
            this.$gmapApiPromiseLazy().then(gmap => {
                let directionService = new gmap.maps.DirectionsService();
                return new Promise(resolve => {
                    directionService.route({
                        destination: {lat: this.landmarks[i].latitude, lng: this.landmarks[i].longitude},
                        origin: {lat: this.location.lat, lng: this.location.lng},
                        travelMode: "DRIVING"
                    }, resolve);
                }).then(result => {
                    if(result.routes[0].legs != undefined)
                    {
                        this.landmarks[i].drivingDistance = (result.routes[0].legs[0].distance.value / 1609.5024).toFixed(2) + " mi";
                        this.landmarks[i].drivingTime = result.routes[0].legs[0].duration.text;
                    }
                    this.landmarks[i].directDistance = this.distance(this.landmarks[i].latitude, this.landmarks[i].longitude, this.location.lat, this.location.lng).toFixed(2) + " mi";
                    this.computeDisplayedLandmarks();
                });
            });
        },
        distance(lat1, lon1, lat2, lon2) {
            var p = 0.017453292519943295;    // Math.PI / 180
            var c = Math.cos;
            var a = 0.5 - c((lat2 - lat1) * p)/2 + 
                    c(lat1 * p) * c(lat2 * p) * 
                    (1 - c((lon2 - lon1) * p))/2;

            return 7919 * Math.asin(Math.sqrt(a)); // 2 * R; R = 6371 km
        },
        maxDistanceChange()
        {
            this.computeDisplayedLandmarks();
        },
        onTableClick(e)
        {
            this.$router.push({ path: `/landmark/${e.id}` });
        },
        onAddClick(e)
        {
            this.$emit('add-clicked', e);
        },
        HelperRequestMap()
        {
            mapsHelper.RequestMap(this.searchChange);
        }
    }
}
</script>

<style scoped>
  #searchContainer {
    margin-top: 50px;
  }

    label{
        margin-right: 30px;
        margin-left: 10px;
    }
    span{
        margin-right: 25px;
    }
    #searchForm{
        margin-top: 20px;
    }
    .tableLandmark{
        cursor: pointer;
    }
</style>