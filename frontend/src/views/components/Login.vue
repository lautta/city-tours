<template>
  <div>
    <b-button variant='primary' style='margin: 0px;' v-b-modal.loginModal>Sign In</b-button>

    <b-modal id="loginModal" title="Sign In" >
      <b-form>
        <b-form-group id="username-input-group" label="Username: " label-for="username-input">
          <b-form-input id="username-input" v-model="user.username" type="text" required></b-form-input>
        </b-form-group>
        <b-form-group id="password-input-group" label="Password: " label-for="password-input">
          <b-form-input id="password-input" v-model="user.password" type="password" required></b-form-input>
        </b-form-group>
      </b-form>
      <b-alert variant="success" :show='successTime' @dismissed="successTime=0">Your account has been created, you may now log in.</b-alert>
      <b-alert variant="danger" :show='dangerTime' @dismissed="dangerTime=0">Your account could not be created, please try again later.</b-alert>
      <b-alert variant="danger" :show='invalidLoginTime' @dismissed="invalidLoginTime=0">Your username or password was incorrect, please try again.</b-alert>
      <template v-slot:modal-footer="{ signIn, register, cancel }">
        <b-button size="sm" variant="success" @click="login()">Sign In</b-button>
        <b-button size="sm" variant="warning" @click="newUser()">Register</b-button>
        <b-button size="sm" variant="danger" @click="cancel()">Cancel</b-button>
      </template>
    </b-modal>
  </div>
</template>

<script>
import auth from "../../auth";

export default {
  data() {
    return {
      user: {
        username: "",
        password: ""
      },
      successTime: 0,
      dangerTime: 0,
      invalidLoginTime: 0,
      invalidCredentials: true
    };
  },
  methods: {
    login() {
      fetch(process.env.VUE_APP_REMOTE_API + "/api/account/login", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(this.user)
      })
        .then(response => {
          if (response.ok) {
            return response.text();
          } else {
            this.invalidCredentials = true;
          }
        })
        .then(token => {
          if (token != undefined) {
            if (token.includes('"')) {
              token = token.replace(/"/g, "");
            }
            auth.saveToken(token);
            this.clearForm();
            this.$bvModal.hide("loginModal");
            this.$bus.$emit('login', 'logged_in');
          }
          else{
            this.invalidLoginTime = 5;
          }
        })
        .catch(err => console.error(err));
    },
    newUser()
    {
      fetch(process.env.VUE_APP_REMOTE_API + "/api/account/register", {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(this.user),
      })
        .then((response) => {
          if (response.ok) {
            this.successTime = 5;
          } else {
            this.dangerTime = 5;
          }
        })

        .then((err) => console.error(err));
    },
    clearForm(){
        this.username = '';
        this.password = '';
    },
  },
  destroyed: {
    
  }
};
</script>

<style lang="scss" scoped>
</style>