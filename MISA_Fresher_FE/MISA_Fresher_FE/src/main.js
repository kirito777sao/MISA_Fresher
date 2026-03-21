import './assets/css/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Antd from 'ant-design-vue';
import App from './App.vue'
import router from './router'

const pinia = createPinia();
const app = createApp(App);

app.use(router);
app.use(pinia);
app.use(Antd);
app.mount('#app');
