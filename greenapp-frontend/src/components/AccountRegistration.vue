<template>
  
  <div id="reg-container" v-if="!isLoggedIn">
    <div>
      <h2>Register account</h2>
      <form @submit.prevent="register">

        <input type="text" id="username" placeholder="Username" v-model="user.username" required>
        <br>

        <input type="password" id="password" placeholder="Password" v-model="user.password" required>
        <br>

        <input type="email" id="email" placeholder="Email Address" v-model="user.email" required>
        <br>
        <button type="submit" id="register-button">Register</button>
      </form>
    </div>
  </div>
  <div v-else id="logged-in-message">
    <p>You are already logged in.</p>
  </div>
  <div>
    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
  </div>

</template>

<script>
import axios from 'axios';

export default {
  name: 'RegistrationForm',
  components: {
  },
  data() {
    return {
      user: {
        username: '',
        password: '',
        email: ''
      },
      errorMessage: ''
    }
  },
  computed: {
    isLoggedIn() {
      return !!this.$store.getters.getToken; 
    }
  },
  methods: {
    async register() {
      try {
        const response = await axios.post('https://localhost:7075/Register', this.user);
        console.log(response.data);
      } catch (error) {
        if (error.response.status === 409) {
          if (error.response.data === 'Username') {
            this.errorMessage = 'Username already exists';
          } else if (error.response.data === 'Email') {
            this.errorMessage = 'Email already registered';
          } else if(error.response.data === "ShortPassword"){
            this.errorMessage = 'Password needs to be 8 characters or more';
          }
          else {
            this.errorMessage = 'Conflict occurred';
          }
        } else {
          console.error('Error registering:', error);
        }
      }
    }
  },
  created() {
  }
}
</script>

<style scoped>
#reg-container {
  display: flex;
  justify-content: center;
  align-items: center;
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

#register-button {
  margin-top: 10px;
  padding: 10px 20px;
  font-size: 16px;
  background-color: #383838;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

#register-button:hover {
  background-color: #000000;
}

#register-button:focus {
  outline: none;
}
.error-message{
  text-align: center;
}
#logged-in-message{
  text-align:center;
}
</style>