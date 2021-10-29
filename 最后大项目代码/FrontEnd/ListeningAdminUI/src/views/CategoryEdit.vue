<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="中文标题">
    <el-input v-model="state.data.name.chinese"></el-input>
  </el-form-item>
  <el-form-item label="英文标题">
    <el-input v-model="state.data.name.english"></el-input>
  </el-form-item>  
  <el-form-item label="封面图片">
    <el-input v-model="state.data.coverUrl"></el-input>
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
  name: 'CategoryEdit',
  setup(){
    const state=reactive({data:{name:{chinese:"",english:""},coverUrl:""}});  
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const id = router.currentRoute.value.query.id;//读取页面参数
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Listening.Admin/Category/FindById/${id}`);
      state.data = data;
    });
    const save=async ()=>{      
      await axios.put(`${apiRoot}/Listening.Admin/Category/Update/${id}`,state.data);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>