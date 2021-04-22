import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import axios from "axios";
import Toaster from '@meforma/vue-toaster';
import mitt from 'mitt';
import EventHub from './eventHub';

axios.interceptors.request.use(request => {
    if (store.state.context.jwtToken) {
        request.headers['Authorization'] = 'Bearer ' + store.state.context.jwtToken;
    }
    return request;
  });

const app = createApp(App)
  .use(router)
  .use(store)
  .use(Toaster)
  .mixin({
    created(){
      let opts = { position: "bottom-right" };
      this.$notifyError = (text) => this.$toast.error(text.toString(), opts);
      this.$notifySuccess = (text) => this.$toast.success(text.toString(), opts);
      this.$notifyNormal = (text) => this.$toast.show(text.toString(), opts);
      this.$notifyInfo = (text) => this.$toast.info(text.toString(), opts);
    }
  });
const emitter = mitt();
app.config.globalProperties.emitter = emitter;
app.use(EventHub);
app.mount('#app');

store.dispatch("users/getUsers");
store.dispatch("global/loadHome");