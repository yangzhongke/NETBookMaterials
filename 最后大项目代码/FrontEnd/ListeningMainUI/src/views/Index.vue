<template>
  <el-input placeholder="输入搜索内容" v-model="state.searchWord">
    <template #append>
      <el-button icon="el-icon-search" v-on:click="doSearch()"></el-button>
    </template>
  </el-input> 
  <el-row :gutter="10">
  <el-col :span="6" v-for="c in state.categories" v-bind:key="c.id">
    <router-link :to="{path:'Category',query:{id:c.id}}">
      <div style="text-align:center;">
        <el-image :src="c.coverUrl"></el-image>     
        <div class="title">{{c.name.chinese}}</div>
      </div>  
    </router-link>
  </el-col>
</el-row>
</template>

<script>
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Index',
  setup(){
    const state=reactive({categories:[],searchWord:""});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Category/FindAll`);
      state.categories = data;
    });
    const doSearch = ()=>{
      router.push({name: 'Search',query:{word:state.searchWord}});
    };
	  return {state,doSearch};
  },
}
</script>
<style scoped>
</style>