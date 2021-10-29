import { createApp } from 'vue'
import ElementPlus from 'element-plus';
import 'element-plus/lib/theme-chalk/index.css';
import axios from 'axios';
import App from './App.vue'
import router from './route'

const app = createApp(App);
app.use(router).mount('#app');
app.use(ElementPlus);
app.config.globalProperties.apiRoot="http://localhost/Listening.Main";
app.config.globalProperties.searchApiRoot="http://localhost/SearchService";

//全局自动处理ajax异常
//在项目中，校验以及显示定制错误信息应该由客户端负责，所以提供客户可读错误信息不应该由服务器端负责
//由于这个项目重点是服务器端，所以客户端就这样简单处理了。
axios.interceptors.response.use(function (response) {
    return response;
}, function (err) {
    const status = err.response.status;
    const text = JSON.stringify(err.response.data);
    alert(`Ajax错误，状态码${status}，报文体：${text}`);
    //如果发现请求出现权限错误，则也在这里自动重定向到登录界面
    throw err;
});