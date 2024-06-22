<template>
  <div>
    <MainHeader @navigate-to-registration="register" @search-criteria="updateSearchCriteria" @reset-search="resetSearch" />
    <router-view :key="viewKey" :search-criteria="searchCriteria" />
  </div>
</template>

<script>
import MainHeader from './components/MainHeader.vue';

export default {
  name: 'App',
  components: {
    MainHeader
  },

  data() {
    return {
      searchCriteria: {
        searchPhrase: '',
        color: 'all'
      },
      viewKey: 0
    };
  },
  
  methods: {
    register() {
      this.$router.push('/register');
    },

    updateSearchCriteria(criteria) {
      this.searchCriteria = criteria;
    },

    resetSearch() {
      this.viewKey += 1;
      this.searchCriteria = { searchPhrase: '', color: 'all' };
    }

  }
}
</script>

<style> 
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 75px;
}
</style>
