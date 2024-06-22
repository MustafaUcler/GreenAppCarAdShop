<template>
  <div class="car-details" v-if="carDetails">
    <h1>{{ carDetails.brand }} {{ carDetails.modelName }}</h1>
    <img v-if="carDetails.guid" :src="'https://localhost:7075/images/' + carDetails.guid" alt="Car Image" class="car-image" />
    <p><strong>Color:</strong> {{ carDetails.color }}</p>
    <p><strong>Mileage:</strong> {{ carDetails.mileage }} km</p>
    <p><strong>Fuel Type:</strong> {{ carDetails.fuelType }}</p>
    <p><strong>Gearbox:</strong> {{ carDetails.gearbox }}</p>
    <p><strong>Price:</strong> ${{ carDetails.price }}</p>
    <p><strong>Horsepower:</strong> {{ carDetails.horsepower }}</p>
    <p><strong>Production Year:</strong> {{ carDetails.productionYear }}</p>
    <p><strong>Description:</strong> {{ carDetails.description }}</p>
    <p>Posted on: {{ formatDate(carDetails.publishDate) }}</p>

    <button @click="purchaseListing" class="purchase-button">Purchase</button>
    <!-- Contact Seller Button -->
    <button @click="contactSeller(carDetails)" class="contact-button">Contact Seller</button>

    <!-- Favorite Button -->
    <button @click="toggleFavorite(carDetails)" :class="{ 'favorited': carDetails.isFavorited }"
      class="favorite-button">Favorite</button>

  </div>
</template>

<script>
import axios from 'axios';
import { format, parseISO } from 'date-fns';



export default {
  props: {
    carId: {
      type: [Number, String],
      required: true
    }
  },

  data() {
    return {
      carDetails: null // Initialize car details
    };
  },

  created() {
    this.fetchCarDetails();
  },

  methods: {

    async fetchCarDetails() {
      try {
        const token = this.$store.getters.getToken;
        const response = await axios.get(`https://localhost:7075/GetCarDetails/${this.carId}`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        this.carDetails = response.data;

      } catch (error) {
        console.error('Failed to fetch car details:', error);
      }
    },

    formatDate(dateString) {
      return format(parseISO(dateString), 'dd-MM-yyyy');
    },

    async purchaseListing() {
      const token = this.$store.getters.getToken;

      try {
        await axios.get('https://localhost:7075/Purchase', {
          params: {
            carId: this.carId 
          },
          headers: {
            Authorization: `Bearer ${token}` 
          }
        });
        console.log('Purchase request sent successfully');
      } catch (error) {
        console.error('Error purchasing listing:', error);
      }

    },
    getCarImageUrl(guid) {
      return `https://localhost:7075/images/${guid}`;
    },
  }
};

</script>

<style scoped>
.car-details {
  border: 1px solid #ccc;
  border-radius: 5px;
  padding: 20px;
  margin-bottom: 20px;
}

.car-details img {
  width: 40%;
  height: auto;
}

.favorite-button.favorited {
  background-color: yellow;
}

.contact-button,
.favorite-button,
.purchase-button {
  margin-top: 10px;
  padding: 5px 10px;
  cursor: pointer;
}
</style>