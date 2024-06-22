<template>
  <div v-if="isUserLoggedIn()" id="listings-container">
    <h2>Publish new listing</h2>
    <form @submit.prevent="submitListing" id="listings-form" class="form-grid">
      <div class="form-column">
        <input type="text" v-model="newListing.color" placeholder="Färg">
        <input type="text" v-model="newListing.brand" placeholder="Märke">
        <input type="text" v-model="newListing.modelName" placeholder="Modellnamn">
        <input type="number" v-model="newListing.milage" placeholder="Mätarställning">
      </div>
      <div class="form-column">
        <input type="text" v-model="newListing.fuelType" placeholder="Bränsletyp">
        <input type="text" v-model="newListing.gearbox" placeholder="Växellåda">
        <input type="number" v-model="newListing.price" placeholder="Pris">
        <input type="number" v-model="newListing.horsepower" placeholder="Hästkrafter">
        <input type="number" v-model="newListing.productionYear" placeholder="Tillverkningsår">
        <input type="file" accept="image/*" @change="handleFileUpload" />
      </div>
      <textarea v-model="newListing.description" placeholder="Beskrivning" rows="5"></textarea>
      <button type="submit">Publish</button>
    </form>
  </div>
  <div v-else>
    <p>Du måste vara inloggad för att lägga till en annons. Vänligen logga in först.</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      newListing: {
        color: '',
        brand: '',
        modelName: '',
        milage: null,
        fuelType: '',
        gearbox: '',
        price: null,
        horsepower: null,
        productionYear: null,
        description: '',
      },
      selectedFile: null

    };
  },
  methods: {
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0];
      console.log(this.selectedFile);
    },
    isUserLoggedIn() {
      const token = localStorage.getItem('token');
      return token !== null;
    },
    async submitListing() {
      if (!this.isUserLoggedIn()) {
        return;
      }
      //FormData
      const formData = new FormData();
      formData.append('color', this.newListing.color);
      formData.append('brand', this.newListing.brand);
      formData.append('modelName', this.newListing.modelName);
      formData.append('milage', this.newListing.milage);
      formData.append('fuelType', this.newListing.fuelType);
      formData.append('gearbox', this.newListing.gearbox);
      formData.append('price', this.newListing.price);
      formData.append('horsepower', this.newListing.horsepower);
      formData.append('productionYear', this.newListing.productionYear);
      formData.append('description', this.newListing.description);

      if (this.selectedFile) {
        formData.append('file', this.selectedFile);
      } else {
        console.error('No file selected');
        return; // or handle the error as needed
      }
      //FormData
      const token = this.$store.getters.getToken;
      try {
        const response = await axios.post('https://localhost:7075/Listing', formData, {
          headers: {
            Authorization: `Bearer ${token}`, // Include the token in the request headers
            'Content-Type': 'multipart/form-data'

          }
        });

        console.log(response);
        this.resetForm();

      } catch (error) {
        if (error.response.status === 409) {
          if (error.response.data === 'Expired') {
            this.logout();
          }
        }
      }
    },
    resetForm() {
      // Återställ formuläret efter att annonsen har lagts till
      this.newListing = {
        color: '',
        brand: '',
        modelName: '',
        milage: null,
        fuelType: '',
        gearbox: '',
        price: null,
        horsepower: null,
        productionYear: null,
        description: ''
      };
    },

    logout() {
      // Clear token and username from Vuex store and local storage upon logout
      this.$store.commit('setToken', null);
      this.$store.commit('setUsername', null);
      localStorage.removeItem('token')
      localStorage.removeItem('username')
    }

  }
};
</script>

<style scoped>
.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.form-column {
  display: flex;
  flex-direction: column;
}

textarea {
  grid-column: 1 / span 2;
}

button {
  grid-column: 1 / span 2;
  justify-self: center;
}

form input {
  margin-top: 10px;
  padding: 15px;
  font-size: 18px;
  border: 2px solid #ccc;
  border-radius: 5px;
  outline: none;
  transition: border-color 0.3s ease;
}

form input:focus {
  border-color: #000000;
}
</style>