<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="用户名">
    <span>{{state.userName}}</span>
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
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
export default {
  name: 'AdminUserEdit',
  setup(){
    const router = useRouter();
    const id = router.currentRoute.value.query.id;//读取页面参数
    const state=reactive({userName:"",phoneNum:""});  
    const {apiRoot} = getCurrentInstance().proxy;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/IdentityService/UserAdmin/FindById/${id}`);
      state.userName = data.userName;
      state.phoneNum = data.phoneNumber;
    });
    const save=async ()=>{      
      await axios.put(`${apiRoot}/IdentityService/UserAdmin/UpdateAdminUser/${id}`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>