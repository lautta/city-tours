<template>
  <div id="app">
    <b-navbar id='mainNav' toggleable="lg" type="dark" variant="info">
    <b-navbar-brand href="/">City Tours</b-navbar-brand>
    <b-navbar-nav>
        <b-nav-item href="/search-landmarks">Search Landmarks</b-nav-item>
        <b-nav-item href="/itinerary">Create an Itinerary</b-nav-item>
        <b-nav-item v-if='signedIn' href="/add-landmark"><span v-if="admin" >Add a Landmark</span><span v-else >Suggest A Landmark</span></b-nav-item>
        <b-nav-item href="/approve-landmarks" v-if='admin'>Landmark Management</b-nav-item>
    </b-navbar-nav>
    <b-navbar-nav class='ml-auto'>      
       <b-nav-item-dropdown
        id="my-nav-dropdown"
        text="Themes"
        toggle-class="nav-link-custom"
        right
      >
        <b-dropdown-item @click='ChangeTheme'>Default</b-dropdown-item>
        <b-dropdown-item @click='ChangeTheme'>Dark</b-dropdown-item>
        <b-dropdown-item @click='ChangeTheme'>Green</b-dropdown-item>
    </b-nav-item-dropdown>
        <b-nav-item v-if='signedIn' @click='SignOut' href="/">Sign Out</b-nav-item>
        <Login v-else></Login>
        <!--<b-nav-item v-if='!signedIn' href="/login">Sign In</b-nav-item>-->
    </b-navbar-nav>
  </b-navbar>
    <router-view/>
  </div>
</template>
<script>
import auth from './auth';
import Login from './views/components/Login'
export default {
  data() {
        return {
            
            signedIn: auth.getUser() != null,
            admin: false
        }
  },
  methods:
  {
    SignOut()
    {
      auth.logout();
      console.log(this.signedIn = auth.getUser() != null);
    },
    SelectTheme(theme) {
      
        localStorage.setItem('theme', theme);
        this.$root.mapOptions = this.$root.mapChoices[theme];
        this.$root.loadingPath = theme;
        this.RemoveTheme();
        let themeLink = document.createElement("link");
        themeLink.setAttribute("rel", "stylesheet");
        themeLink.setAttribute("href", "/css/" + theme + ".css");
        themeLink.setAttribute("id", "theme-stylesheet");

        let docHead = document.querySelector("head");
        docHead.append(themeLink);
    },
    RemoveTheme() {
      let themeLink = document.querySelector("#theme-stylesheet");
      if(themeLink != null)
      {
        let parentNode = themeLink.parentNode;
        parentNode.removeChild(themeLink);
      }
    },
    ChangeTheme(e)
    {
      this.SelectTheme(e.target.text.toLowerCase());
    }
  },
  created(){
    if(localStorage.getItem('theme') != null)
    {
      this.SelectTheme(localStorage.getItem('theme'));
    }
    else{
      this.SelectTheme('default');
    }


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
          this.signedIn = true;
        }
    })
  },
  components: {
    Login
  }
}
</script>

<style>
@import url('https://fonts.googleapis.com/css?family=PT+Sans');

#app {
  font-family: "PT Sans", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
</style>