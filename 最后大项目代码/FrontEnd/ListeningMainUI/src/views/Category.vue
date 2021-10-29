<template>
  <el-row :gutter="10" v-for="a in state.albums" v-bind:key="a.id">
      <router-link :to="{path:'Album',query:{id:a.id}}">
        {{a.name.chinese}}
      </router-link>   
      <el-divider></el-divider>
  </el-row>
</template>

<script>
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Category',
  setup(){
    const state=reactive({categories:[]});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    var id = router.currentRoute.value.query.id;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Album/FindByCategoryId/${id}`);
      state.albums = data;
    });
	  return {state};
  },
}
</script>
<style scoped>
</style>