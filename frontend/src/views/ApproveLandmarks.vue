<template>
  <b-container id="approveContainer">
    <h1>Approve or Delete Landmarks</h1>
    <p style="text-align: center" v-if="noLandmarks">There are no landmarks to approve.</p>
    <b-table
      v-if="!noLandmarks"
      class="approvalTable"
      striped
      hover
      :items="unapprovedLandmarks"
      :fields="unapprovedFields"
    >
      <template v-slot:cell(approve)="row">
        <b-button size="sm" @click="approveLandmark(row)" class="mr-1">Approve</b-button>
      </template>
      <template v-slot:cell(deny)="row">
        <b-button size="sm" @click="denyLandmark(row)" class="mr-1">Deny</b-button>
      </template>
    </b-table>
    <b-table class="landmarkTable" striped hover :items="landmarks" :fields="displayedFields">
      <template v-slot:cell(delete)="row">
        <b-button size="sm" @click="deleteLandmark(row)" class="mr-1">Delete</b-button>
      </template>
    </b-table>
  </b-container>
</template>

<script>
import auth from "../auth";
export default {
  data() {
    return {
      noLandmarks: false,
      unapprovedLandmarks: [],
      landmarks: [],
      displayedFields: [
        "longName",
        "latitude",
        "longitude",
        "streetNumber",
        "streetName",
        "city",
        "state",
        "zip",
        "details",
        "delete"
      ],
      unapprovedFields: [
        "longName",
        "latitude",
        "longitude",
        "streetNumber",
        "streetName",
        "city",
        "state",
        "zip",
        "details",
        "approve",
        "deny"
      ]
    };
  },
  methods: {
    getAllLandmarks() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/approved`, {
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
            this.landmarksNotFound();
          }
        })
        .then(landmarkList => {
          if (landmarkList.length > 0) {
            this.landmarks = landmarkList;
          } else {
            this.landmarksNotFound();
          }
        })
        .catch(err => {
          console.log(err);
          this.landmarksNotFound();
        });
    },
    getAllUnapprovedLandmarks() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/unapproved`, {
        method: "GET",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: `Bearer ${auth.getToken()}`
        }
      })
        .then(response => {
          if (response.ok) {
            return response.json();
          } else {
            this.landmarksNotFound();
          }
        })
        .then(landmarkList => {
          if (landmarkList.length > 0) {
            this.unapprovedLandmarks = landmarkList;
          } else {
            this.landmarksNotFound();
          }
        })
        .catch(err => {
          console.log(err);
          this.landmarksNotFound();
        });
    },
    landmarksNotFound() {
      this.noLandmarks = true;
    },
    approveLandmark(row) {
      fetch(
        `${process.env.VUE_APP_REMOTE_API}/api/landmark/approve/` + row.item.id,
        {
          method: "POST",
          headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            Authorization: `Bearer ${auth.getToken()}`
          }
        }
      ).catch(err => {
        console.log(err);
      });
      this.removeUnapprovedLandmark(row.item.id);
      //this.getAllUnapprovedLandmarks();
      //this.getAllLandmarks();
    },
    denyLandmark(row) {
      fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/` + row.item.id, {
        method: "DELETE",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: `Bearer ${auth.getToken()}`
        }
      }).catch(err => {
        console.log(err);
      });
      this.removeUnapprovedLandmark(row.item.id);
      //this.getAllUnapprovedLandmarks();
      //this.getAllLandmarks();
    },
    deleteLandmark(row) {
      fetch(`${process.env.VUE_APP_REMOTE_API}/api/landmark/` + row.item.id, {
        method: "DELETE",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: `Bearer ${auth.getToken()}`
        }
      }).catch(err => {
        console.log(err);
      });
      this.removeLandmark(row.item.id);
      //this.getAllUnapprovedLandmarks();
      //this.getAllLandmarks();
    },
    removeLandmark(id) {
      for (let index = 0; index < this.landmarks.length; index++) {
        if (this.landmarks[index].id === id) {
          this.landmarks.splice(index, 1);
        }
      }
    },
    removeUnapprovedLandmark(id) {
      for (let index = 0; index < this.unapprovedLandmarks.length; index++) {
        if (this.unapprovedLandmarks.length === 1) {
          this.unapprovedLandmarks.pop();
        }
        if (this.unapprovedLandmarks[index].id === id) {
          this.unapprovedLandmarks.splice(index, 1);
        }
      }
    }
  },
  created() {
    this.getAllUnapprovedLandmarks();
    this.getAllLandmarks();
    if(auth.getUser() != null)
    {
       if(auth.getUser().rol == 'admin')
          {
            this.admin = true;
          }
    }
  }
};
</script>

<style scoped>
#approveContainer {
  margin-top: 50px;
}
</style>