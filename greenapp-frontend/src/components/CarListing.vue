<template>
    <div>
      <!-- Include the MainHeader -->
      <MainHeader @search-criteria="updateSearchCriteria" @reset-search="resetAndFetchCars" />
  
      <div class="car-listing">
        </div>
           <!-- List of cars -->
      <div v-if="cars.length > 0">
        <div v-for="car in cars" :key="car.listingId" @click="goToCarDetails(car.listingId)" class="car-card">
          <div class="car-details">
            <img v-if="car.guid" :src="'https://localhost:7075/images/' + car.guid" alt="Car Image" class="car-image" />
            <h3>{{ car.brand }} {{ car.modelName }}</h3>
            <p>Color: {{ car.color }}</p>
            <p>Milage: {{ car.milage }}</p>
            <p>Fuel Type: {{ car.fuelType }}</p>
            <p>Gearbox: {{ car.gearbox }}</p>
            <p>Price: {{ car.price }}</p>
            <p>Production Year: {{ car.productionYear }}</p>
            <p>Posted on: {{ formatDate(car.publishDate) }}</p>
            <!-- Contact button -->
            <button @click.stop="contactSeller(car)" class="contact-button">Contact Seller</button>
            <!-- Favorite/Watchlist button -->
            <button @click.stop="toggleFavorite(car)" :class="{ 'favorited': isFavorite(car) }" class="favorite-button">Favorite</button>
          </div>
        </div>
      </div>
      <div v-else>
        <p>No cars for sale right now. Please check back later.</p>
      </div>
      </div>
  </template>
  
  <script>
  import axios from 'axios';
  import MainHeader from './MainHeader.vue'; 
  import { format, parseISO } from 'date-fns';
 
  export default {
    components: { MainHeader }, 
    props: ['searchCriteria'],
    data() {
      return {
        cars: [],
        favorites: [],
        localSearchCriteria: { ...this.searchCriteria }
      };
    },
    created() {
      this.fetchCars(); // Fetch cars data from the backend
      this.loadFavorites(); // Load favorite cars from local storage
    },
    
    watch: {
      $route(to) {
      if (to.name === 'car-listings') {
        console.log('Route change detected. Resetting search criteria.');
        this.resetSearchCriteria();
        this.fetchCars();
      }
    },

    searchCriteria: {
      handler(newCriteria) {
        this.localSearchCriteria = { ...newCriteria };
        this.fetchCars(); 
      },
      deep: true
    }
  },

    methods: {
      async fetchCars() {
        try {
        const endpoint = this.localSearchCriteria.searchPhrase || this.localSearchCriteria.color !== 'all'
          ? 'https://localhost:7075/api/Search/SearchListings'
          : 'https://localhost:7075/GetListings';
          
          const params = {
          searchPhrase: this.localSearchCriteria.searchPhrase || '',
          color: this.localSearchCriteria.color || 'all'
        };

        const response = await axios.get(endpoint, { params });
        
        this.cars = response.data;

        } catch (error) {
          console.error('Complete error details:', error);
        }
      },
      updateSearchCriteria(criteria) {
      this.localSearchCriteria = criteria;  
      this.fetchCars();
      },

      goToCarDetails(carId) {
      this.$router.push({ name: 'car-detail', params: { carId } });
      },
      
      resetAndFetchCars() {
      this.localSearchCriteria = { searchPhrase: '', color: 'all' };
      this.fetchCars();
      },

      // Load favorite cars 
      loadFavorites() {
        // Implement loading favorite cars
      },
      // Save favorite cars 
      saveFavorites() {
        // Implement saving favorite cars
      },
      // Check if a car is in favorites
      isFavorite(car) {
        return this.favorites.some(fav => fav.id === car.id);
      },
      // Toggle favorite status of a car
      toggleFavorite(car) {
        if (this.isFavorite(car)) {
          this.favorites = this.favorites.filter(fav => fav.id !== car.id);
        } else {
          this.favorites.push(car);
        }
        this.saveFavorites();
      },
      // Handle contact seller button click
      contactSeller(car) {
        console.log('Contact seller:', car);
      },
    
      formatDate(dateString) {
         return format(parseISO(dateString), 'dd-MM-yyyy'); // Format only shows date
       }

     },
     beforeRouteEnter(to, from, next) {
     next(vm => {
      if (to.name === 'car-listings') {
        vm.resetAndFetchCars();
      }
      vm.fetchCars();
    });
  }
  
};
  </script>
  
  <style scoped>
  .car-listing {
    padding: 20px;
  }
  
  .car-card {
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 20px;
    margin-bottom: 20px;
    display: flex;
    align-items: center;
  }
  
  .car-card img {
    width: 450px;
    height: auto;
    margin-right: 20px;
  }
  
  .car-details h3 {
    margin-top: 0;
  }
  
  .car-details p {
    margin: 0;
  }
  
  .contact-button,
  .favorite-button {
    margin-top: 10px;
    padding: 5px 10px;
    cursor: pointer;
  }
  
  .favorite-button.favorited {
    background-color: yellow;
  }
  </style>
  