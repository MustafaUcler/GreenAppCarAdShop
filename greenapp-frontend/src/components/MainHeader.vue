<template>
  <header>
    <h1><a href="/" id="home-button">Green App</a></h1>
    <div v-if="isLoggedIn">
    <form @submit.prevent="emitSearchCriteria" class="search-container">
      <input 
        type="text" 
        v-model="searchQuery" 
        placeholder="Search for cars" 
        class="search-input"
      >
      <select v-model="selectedColor" class="color-select">
        <option value="all">All</option>
        <option value="red">Red</option>
        <option value="blue">Blue</option>
        <option value="black">Black</option>
        <option value="white">White</option>
        <option value="yellow">Yellow</option>
        <!-- Add more color options as needed -->
      </select>
      <button type="submit" class="search-button">Search</button>
    </form>
  </div>
    <nav>
      <form @submit.prevent="login" v-if="!isLoggedIn">
        <input type="text" placeholder="Username" v-model="user.username">
        <input type="password" placeholder="Password" v-model="user.password">
        <button type="submit" id="login-button">Login</button>
      </form>
      <button @click="navigateToCarListing" v-if="isLoggedIn" id="car-listing-button">Car Listings</button>
      <router-link v-if="isLoggedIn" to="/listing" id="add-listing-link">Add new listing</router-link>
      <router-link id="router-link" v-if="isLoggedIn"
        :to="'/profile/' + loggedInUsername">{{ loggedInUsername }}</router-link>
      <button @click="navigateToRegistration" id="register-button" v-if="!isLoggedIn">Register</button>



      <div v-show="isLoggedIn" id="logged-in">
        <button @click="logout" id="logout-button">Logout</button>
      </div>
    </nav>

  </header>
  <div v-if="errorMessage" class="chatbox">
    <p>{{ errorMessage }}</p>

  </div>


</template>

<script>
import axios from 'axios';


export default {
  emits: ['reset-search','search-criteria','navigate-to-registration'],
  data() {
    return {
      user: {
        username: '',
        password: ''
      },
      searchQuery: '',
      selectedColor: 'all', 
      errorMessage: '',
     
    };
  },
  computed: {
    isLoggedIn() {
      return !!this.$store.getters.getToken;
    },
    loggedInUsername() {
      return this.$store.getters.getUsername;
    },

  },
  methods: {
    async login() {
      try {
        const response = await axios.post('https://localhost:7075/Login', this.user);

        //Store authentication token and username in vuex store

        this.$store.dispatch('saveToken', { token: response.data.token, username: response.data.username });

        this.$router.push('/');

      } catch (error) {
        if (error.response.status === 409) {

          if (error.response.data === 'UsernameOrPassword') {
            this.errorMessage = 'Invalid password or username';
          }
        }
        else {
          console.error('Error logging in:', error.response.data);
        }
        this.showError();
      }
    },

    showError() {
      setTimeout(() => {
        this.errorMessage = '';
      }, 5000);
    },
    navigateToRegistration() {
      this.$emit('navigate-to-registration');
    },
    logout() {
      // Clear token and username from Vuex store and local storage upon logout
      this.$store.commit('setToken', null);
      this.$store.commit('setUsername', null);
      localStorage.removeItem('token')
      localStorage.removeItem('username')
    },
    navigateToCarListing() {
      this.$emit('reset-search');
      this.$router.push('/CarListings');
      
    },

    emitSearchCriteria() {
      this.$emit('search-criteria', { searchPhrase: this.searchQuery, color: this.selectedColor });
    }
  }
}
</script>

<style scoped>
header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 30px;
  background-color: #002501;
  color: #fff;
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

h1 {
  margin: 0;
}

nav {
  display: flex;
  align-items: center;
}

nav form,
#logged-in {
  display: flex;
  align-items: center;
  margin-left: auto;
}

#logged-in,
#logged-in-username {
  margin-right: 2.5rem;
}

input[type="text"],
input[type="password"],
#login-button {
  height: 30px;
  margin-right: 0.5rem;
}

#register-button {
  height: 30px;
  margin-right: 2.5rem;
}

#home-button {
  color: #fff;
  text-decoration: none;
  cursor: pointer;
}

.chatbox {
  position: fixed;
  bottom: 20px;
  right: 20px;
  background-color: rgba(255, 0, 0, 0.8);
  color: #fff;
  padding: 10px;
  border-radius: 5px;
  animation: slideIn 0.5s ease;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
  }

  to {
    transform: translateX(0);
  }
}

#router-link {
  color: #fff;
  text-decoration: none;
  cursor: pointer;
  margin-right: 2.5rem;
}

#add-listing-link {
  color: #fff;
  text-decoration: none;
  cursor: pointer;
  margin-right: 50px;
}

.search-input {
  height: 30px;
  border: none;
  border-radius: 5px;
  padding: 0 10px;
}

#car-listing-button {
  margin-right: 50px;
  background: none;
  border: none;
  padding: 0;
  font: inherit;
  cursor: pointer;
  color: white;
}
</style>
