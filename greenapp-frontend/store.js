import { createStore } from 'vuex';

export default createStore({
  state() {
    return {
      token: null,
      username: null
    };
  },
  mutations: {
    setToken(state, token) {
      state.token = token;
    },
    setUsername(state, username) { 
      state.username = username;
    }
  },
  actions: {
    saveToken({ commit }, { token, username }) { 
      commit('setToken', token);
      commit('setUsername', username);
      localStorage.setItem('token', token);
      localStorage.setItem('username', username);
    }//Also store token and username in localstorage to persist reload of page (?)
  },
  getters: {
    getToken(state) {
      return state.token;
    },
    getUsername(state) { 
      return state.username;
    }
  }
});
