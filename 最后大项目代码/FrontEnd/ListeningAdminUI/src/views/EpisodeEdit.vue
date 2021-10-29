<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="中文标题">
    <el-input v-model="state.data.name.chinese"></el-input>
  </el-form-item>
  <el-form-item label="英文标题">
    <el-input v-model="state.data.name.english"></el-input>
  </el-form-item>  
  <el-form-item label="字幕类型">
    <el-select v-model="state.data.subtitleType" placeholder="请选择字幕类型">
      <el-option label="srt" value="srt"></el-option>
      <el-option label="vtt" value="vtt"></el-option>
      <el-option label="lrc" value="lrc"></el-option>
      <el-option label="json" value="json"></el-option>
    </el-select>
  </el-form-item> 
  <el-form-item label="字幕">
    <el-input type="textarea" v-model="state.data.subtitle" rows="10"></el-input>
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
  name: 'EpisodeEdit',
  setup(){
    const router = useRouter();
    const id = router.currentRoute.value.query.id;//读取页面参数
    const state=reactive({data:{name:{chinese:"",english:""},id:id,subtitleType:"srt",
      subtitle:""}});      
    const {apiRoot} = getCurrentInstance().proxy;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Listening.Admin/Episode/FindById/${id}`);
      state.data = data;
    });
    const save=async ()=>{
      await axios.put(`${apiRoot}/Listening.Admin/Episode/Update/${id}`,state.data);
        history.back();     
    };
    const playerDurationChange = ()=>{
        var player = document.getElementById("player");
        state.data.durationInSecond = player.duration;
    };
	  return {state,save,playerDurationChange};
  },
}
</script>
<style scoped>
</style>