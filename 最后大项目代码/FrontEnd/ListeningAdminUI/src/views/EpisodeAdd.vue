<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="中文标题">
    <el-input v-model="state.name.chinese"></el-input>
  </el-form-item>
  <el-form-item label="英文标题">
    <el-input v-model="state.name.english"></el-input>
  </el-form-item>  
  <el-form-item label="音频路径">
    <audio id="player" @durationchange="playerDurationChange" style="display:none" :src="state.audioUrl" controls></audio>
    <ZackUploader v-model="state.audioUrl"></ZackUploader>
  </el-form-item>   
  <el-form-item label="音频秒数">
    <el-input v-model="state.durationInSecond"></el-input>
  </el-form-item>  
  <el-form-item label="字幕类型">
    <el-select v-model="state.subtitleType" placeholder="请选择字幕类型">
      <el-option label="srt" value="srt"></el-option>
      <el-option label="vtt" value="vtt"></el-option>
	  <el-option label="lrc" value="lrc"></el-option>
	  <el-option label="json" value="json"></el-option>
    </el-select>
  </el-form-item> 
  <el-form-item label="字幕">
    <el-input type="textarea" v-model="state.subtitle" rows="5"></el-input>
  </el-form-item>  
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>
</el-form>
</template>
<script>
import axios from 'axios';
import {reactive, getCurrentInstance} from 'vue';
import {useRouter } from 'vue-router';
import ZackUploader from '../components/ZackUploader.vue';

export default {
  name: 'EpisodeAdd',
  components:{ZackUploader},
  setup(){
    const router = useRouter();
    const albumId = router.currentRoute.value.query.albumId;//读取页面参数
    const state=reactive({name:{chinese:"",english:""},albumId:albumId,audioUrl:"",
      subtitleType:"srt",subtitle:"",durationInSecond:""});  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async()=>{
          await axios.post(`${apiRoot}/Listening.Admin/Episode/Add`,state);
          history.back();  
    };
    const playerDurationChange = ()=>{
        var player = document.getElementById("player");
        state.durationInSecond = player.duration;
    };
	  return {state,save,playerDurationChange};
  },
}
</script>
<style scoped>
</style>