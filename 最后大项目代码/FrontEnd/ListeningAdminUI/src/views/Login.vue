<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="用户名">
    <el-input v-model="state.userName"></el-input>
  </el-form-item>
  <el-form-item label="密码">
    <el-input type="password" v-model="state.password"></el-input>
  </el-form-item>  
  <el-form-item label="验证码图片">
    <el-input v-model="state.captcha"></el-input>
    <el-image :src="state.captchaImgUrl" @click="reloadCaptchaImg"></el-image>
  </el-form-item>    
  <el-form-item>
    <el-button type="primary" @click="login">登录</el-button>
  </el-form-item>
</el-form>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
import { VueCookieNext } from 'vue-cookie-next'

export default {
  name: 'Login',
  setup(){
    const state=reactive({userName:"",password:"",captcha:"",captchaImgUrl:"",ticketId:""});  
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const reloadCaptchaImg=async()=>{
      let r = await axios.post(`${apiRoot}/IdentityService/Login/CreateTicketIdForCaptcha`);
      state.ticketId = r.data;
      state.captchaImgUrl = `${apiRoot}/IdentityService/Login/CreateCaptchaImage?ticketId=${state.ticketId}`;   
    };
    onMounted(async function(){
      reloadCaptchaImg();
    });
    const login=async ()=>{   
      const data = {
        "userName": state.userName,
        "password": state.password,
        "captcha": state.captcha,
        "ticketId": state.ticketId,
      };   
      const jwtToken = await axios.post(`${apiRoot}/IdentityService/Login/LoginByUserNameAndPwd`,data);
      axios.defaults.headers.common['Authorization']="Bearer "+jwtToken.data;
      VueCookieNext.setCookie("Authorization","Bearer "+jwtToken.data);//Authorization保存到cookie中，这样刷新或者退出后仍然能用
      router.push({name:'AdminUserList'});
    }
	  return {state,reloadCaptchaImg,login};
  },
}
</script>
<style scoped>
</style>