import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from "axios";


axios.interceptors.request.use(request => {
    if (store.state.context.jwtToken) {
        request.headers['Authorization'] = 'Bearer ' + store.state.context.jwtToken;
    }
    return request;
  })

createApp(App).use(router).use(store).mount('#app');
