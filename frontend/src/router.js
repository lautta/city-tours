import Vue from 'vue'
import Router from 'vue-router'
import auth from './auth'
import Home from './views/Home.vue'
import Login from './views/Login.vue'
import Register from './views/Register.vue'
import Landmark from './views/Landmark.vue'
import AddLandmark from './views/AddLandmark.vue'
import NotFound from './views/NotFound.vue'
import SearchLandmarks from './views/SearchLandmarks.vue'
import Itinerary from './views/Itinerary.vue'
import ViewItinerary from './views/viewitinerary.vue'
import ApproveLandmarks from './views/ApproveLandmarks.vue'

Vue.use(Router)

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */

const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/login",
      name: "login",
      component: Login,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/register",
      name: "register",
      component: Register,
      meta: {
        requiresAuth: false
      },
    },
    {
      path: "/add-landmark",
      name: "add landmark",
      component: AddLandmark,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/landmark/:id",
      name: "landmark",
      component: Landmark,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/search-landmarks",
      name: "search-landmarks",
      component: SearchLandmarks,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "*",
      name: "not-found",
      component: NotFound,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/itinerary",
      name: "itinerary",
      component: Itinerary,
      meta: {
        requiresAuth: true
      },
    },
    {
      path: "/itinerary/:id",
      name: "itineraryEdit",
      component: Itinerary,
      meta: {
        requiresAuth: true
      },
    },
    {
      path: "/viewitinerary/:id",
      name: "itineraryView",
      component: ViewItinerary,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/approve-landmarks",
      name: "approveLandmark",
      component: ApproveLandmarks,
      meta: {
        requiresAuth: true
      }
    }
  ]
})

router.beforeEach((to, from, next) => {
  // Determine if the route requires Authentication
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);
  const user = auth.getUser();

  // If it does and they are not logged in, send the user to "/login"
  if (requiresAuth && !user) {
    next("/login");
  } else {
    // Else let them go to their next destination
    next();
  }
});

export default router;
