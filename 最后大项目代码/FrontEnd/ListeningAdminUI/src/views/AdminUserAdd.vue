<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="用户名">
    <el-input v-model="state.userName"></el-input>
  </el-form-item>
  <el-form-item label="手机号">
    <el-input v-model="state.phoneNum"></el-input>
  </el-form-item>    
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>  
</el-form>
</template>

<script>
import axios from 'axios';
import {reactive, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
export default {
  name: 'AdminUserAdd',
  setup(){
    const router = useRouter();
    const state=reactive({phoneNum:""});  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async ()=>{      
      await axios.post(`${apiRoot}/IdentityService/UserAdmin/AddAdminUser`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>