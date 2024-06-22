<template>
  <div class="user-profile">
    <h2>User Profile</h2>
    <div class="tabs">
      <button @click="activeTab = 'details'" :class="{ 'active': activeTab === 'details' }">Detaljer</button>
      <button @click="activeTab = 'listing'; fetchUserListings()" :class="{ 'active': activeTab === 'listing' }">Annonser</button>
      <button @click="activeTab = 'favorites'" :class="{ 'active': activeTab === 'favorites' }">Favoriter</button>
      <button @click="activeTab = 'ratings'" :class="{ 'active': activeTab === 'ratings' }">Betyg</button>
      <button @click="activeTab = 'history'; fetchUserHistory()" :class="{ 'active': activeTab === 'history' }">History</button>
    </div>
    <div class="tab-content">
      <div v-if="activeTab === 'details' && userProfileData">
        <p><strong>Namn:</strong> {{ userProfileData.name }}</p>
        <p><strong>Email:</strong> {{ userProfileData.email }}</p>
      </div>
      <div v-else-if="activeTab === 'listing'">
        <div v-if="userListings && userListings.length > 0">
          <h3>Dina annonser:</h3>
          <div v-for="listing in userListings" :key="listing.id">
            <h1>Listing:</h1>
            <p><strong>Färg:</strong> {{ listing.color }}</p>
            <p><strong>Brand:</strong> {{ listing.brand }}</p>
            <p><strong>Model Name:</strong> {{ listing.modelName }}</p>
            <p><strong>Milage:</strong> {{ listing.milage }}</p>
            <p><strong>Fuel Type:</strong> {{ listing.fuelType }}</p>
            <p><strong>Gearbox:</strong> {{ listing.gearbox }}</p>
            <p><strong>Price:</strong> {{ listing.price }}</p>
            <p><strong>Horsepower:</strong> {{ listing.horsepower }}</p>
            <p><strong>Production Year:</strong> {{ listing.productionYear }}</p>
            <p><strong>Description:</strong> {{ listing.description }}</p>
          </div>
        </div>
        <div v-else>
          <p>Du har inga annonser.</p>
        </div>
      </div>
      <div v-else-if="activeTab === 'favorites'">
        <!-- Placera innehållet för fliken "Favoriter" här -->
        <p>Favoriter</p>
      </div>
      <div v-else-if="activeTab === 'ratings'">
        <!-- Placera innehållet för fliken "Betyg" här -->
        <p>Betyg</p>
      </div>
      <div v-else-if="activeTab === 'history'">
        <h1>Your purchases:</h1>
        <div v-for="listing in userHistory" :key="listing.id">
          <h2>Listing: {{ listing.brand }} {{ listing.modelName }}</h2>
            <p><strong>Färg:</strong> {{ listing.color }}</p>
            <p><strong>Brand:</strong> {{ listing.brand }}</p>
            <p><strong>Model Name:</strong> {{ listing.modelName }}</p>
            <p><strong>Milage:</strong> {{ listing.milage }}</p>
            <p><strong>Fuel Type:</strong> {{ listing.fuelType }}</p>
            <p><strong>Gearbox:</strong> {{ listing.gearbox }}</p>
            <p><strong>Price:</strong> {{ listing.price }}</p>
            <p><strong>Horsepower:</strong> {{ listing.horsepower }}</p>
            <p><strong>Production Year:</strong> {{ listing.productionYear }}</p>
            <p><strong>Description:</strong> {{ listing.description }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'UserProfile',

  data() {
    return {
      userProfileData: null,
      userListings: [],
      activeTab: 'details',
      userHistory: []
    };
  },
  created() {
    if (!this.isUserLoggedIn()) {
      this.$router.push('/');
      return;
    }
    this.fetchUserProfile();
  },
  methods: {
    isUserLoggedIn() {
      const token = localStorage.getItem('token');
      return token !== null;
    },
    async fetchUserProfile() {
      try {
        const loggedInUsername = this.$route.params.username;
        const response = await axios.get(`https://localhost:7075/profile/${loggedInUsername}`);
        this.userProfileData = response.data;
      } catch (error) {
        console.error('Error fetching user profile:', error);
      }
    },
    async fetchUserListings() {
      try {
        const token = localStorage.getItem('token');
        const response = await axios.get('https://localhost:7075/ProfileListings', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        this.userListings = response.data;
        console.log('Annonser hämtade:', this.userListings);
      } catch (error) {
        console.error('Fel vid hämtning av användarannonser:', error);
      }
    },
    async fetchUserHistory(){
      const token = this.$store.getters.getToken;
      try{
      const response = await axios.get('https://localhost:7075/History', {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        this.userHistory = response.data;
      }
      
      catch(error){
        console.log("temp");
      }
    }
  }
};
</script>
