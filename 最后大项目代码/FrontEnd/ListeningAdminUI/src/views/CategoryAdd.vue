<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="中文标题">
    <el-input v-model="state.name.chinese"></el-input>
  </el-form-item>
  <el-form-item label="英文标题">
    <el-input v-model="state.name.english"></el-input>
  </el-form-item>  
  <el-form-item label="封面图片">
    <el-input v-model="state.coverUrl"></el-input>
  </el-form-item>    
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>
</el-form>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
export default {
  name: 'CategoryAdd',
  setup(){
    const state=reactive({name:{chinese:"",english:""},coverUrl:""});  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async ()=>{      
      await axios.post(`${apiRoot}/Listening.Admin/Category/Add`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>