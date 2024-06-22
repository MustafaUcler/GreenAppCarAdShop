<template>
    <header>
      <!-- Annonser visas här -->
      <div v-if="paginatedListings.length > 0">

      <h1>Annonser</h1>
      <div v-for="listing in paginatedListings" :key="listing.id">
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
        <img :src="'https://localhost:7075/images/' + listing.guid" alt="Listing Image">
      </div>
  
          <!-- Bläddringsknappar -->
          <div>
          <button @click="previousPage" :disabled="currentPage === 0">Tillbaka</button>
          <button @click="nextPage" :disabled="currentPage === totalPages - 1">Nästa</button>
          </div>
  
    </div>
    </header>

  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {

        listings: [], // tom array för att lagra annonser
        currentPage: 0, // aktuell sida
        listingsPerPage: 1 // antal annonser per sida
      };
    },
    computed: {

        paginatedListings() {
        const reversedListings = this.listings.slice().reverse(); // Skapa en kopia och sortera i omvänd ordning
        const startIndex = this.currentPage * this.listingsPerPage;
        const endIndex = startIndex + this.listingsPerPage;
        return reversedListings.slice(startIndex, endIndex);
        },
      totalPages() {
        return Math.ceil(this.listings.length / this.listingsPerPage);
      }
    },
    methods: {
  
      async fetchListings() { // metod för att hämta annonser
        try {
          const response = await axios.get('https://localhost:7075/GetListings');
          this.listings = response.data;
        } catch (error) {
          console.error('Error fetching listings:', error);
          
        }
        
      },

      nextPage() {
        if (this.currentPage < this.totalPages - 1) {
          this.currentPage++;
        }
      },
      previousPage() {
        if (this.currentPage > 0) {
          this.currentPage--;
        }
      }
    },
    mounted() { // Anropar fetchListings() när komponenten monteras
    this.fetchListings();
  }
  }
  </script>
  
  <style scoped>

  </style>
  