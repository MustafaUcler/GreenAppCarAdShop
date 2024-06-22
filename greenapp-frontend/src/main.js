import { createApp } from 'vue';
import { createRouter, createWebHistory } from 'vue-router';
import App from './App.vue';
import Registration from './components/AccountRegistration.vue';
import CarListing from './components/CarListing.vue';
import store from '/store';
import LandingPage from './components/LandingPage.vue';
import UserProfile from './components/UserProfile.vue';
import AddListing from './components/AddListing.vue';
import CarDetail from './components/CarDetail.vue';


const routes = [
  { path: '/', component: LandingPage },
  { path: '/register', component: Registration },
  { path: '/profile/:username', component: UserProfile },
  { path: '/listing', component: AddListing },
  { path: '/CarListings', component: CarListing },
  { path: '/car/:carId', name: 'car-detail', component: CarDetail, props: true } 

];

const router = createRouter({
  history: createWebHistory(),
  routes
});

const app = createApp(App);

app.use(router);

//vuex store for storing authentication token upon login
app.use(store);

//Get token from local storage to persist upon refreshing...(?)
const token = localStorage.getItem('token');
if (token) {
  store.commit('setToken', token);
}
const username = localStorage.getItem('username');
if(username){
    store.commit('setUsername', username)
}

app.mount('#app');
