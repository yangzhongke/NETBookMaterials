<template>
  <el-row :gutter="10" v-for="e in state.episodes" v-bind:key="e.id">
      <router-link :to="{path:'Episode',query:{id:e.id}}">
        {{e.name.chinese}}
      </router-link>   
      <el-divider></el-divider>
  </el-row>
</template>

<script>
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Album',
  setup(){
    const state=reactive({episodes:[]});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    var id = router.currentRoute.value.query.id;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Episode/FindByAlbumId/${id}`);
      state.episodes = data;
    });
	  return {state};
  },
}
</script>
<style scoped>
</style>