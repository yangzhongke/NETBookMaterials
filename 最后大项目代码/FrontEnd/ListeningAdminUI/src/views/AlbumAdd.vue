<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="中文标题">
    <el-input v-model="state.name.chinese"></el-input>
  </el-form-item>
  <el-form-item label="英文标题">
    <el-input v-model="state.name.english"></el-input>
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
  name: 'AlbumAdd',
  setup(){
    const router = useRouter();
    const categoryId = router.currentRoute.value.query.categoryId;//读取页面参数
    const state=reactive({name:{chinese:"",english:""},categoryId:categoryId});  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async ()=>{      
      await axios.post(`${apiRoot}/Listening.Admin/Album/Add`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>