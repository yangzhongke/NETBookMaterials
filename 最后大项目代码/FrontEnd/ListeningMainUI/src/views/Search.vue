<template>
  <el-input placeholder="输入搜索内容" v-model="state.word">
    <template #append>
      <el-button icon="el-icon-search" v-on:click="doSearch()"></el-button>
    </template>
  </el-input>  
  <el-row :gutter="10" v-for="e in state.episodes" v-bind:key="e.id">
      <router-link :to="{path:'Episode',query:{id:e.id}}">
        {{e.cnName}}
      </router-link>
      <div v-html="e.plainSubtitle"></div>   
      <el-divider></el-divider>
  </el-row>
    <el-pagination
      @current-change="currentPageChange"
      v-model:currentPage="state.currentPage"
      :page-size="10"
      layout="total, prev, pager, next"
      :total="state.totalCount">
    </el-pagination>  
</template>

<script>
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Search',
  setup(){
    const state=reactive({episodes:[],word:"",currentPage:1,totalCount:0});  
    const {searchApiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    var word = router.currentRoute.value.query.word;
    if(word&&word.length>0)
    {
      state.word = word;
    }
    onMounted(async function(){
        if(state.word&&state.word.length>0)
        {
          doSearch();
        }
    });
    const doSearch=async()=>{
      const word = state.word;
      const pageIndex = state.currentPage;
      const {data} = await axios.get(`${searchApiRoot}/Search/SearchEpisodes?Keyword=${word}&PageIndex=${pageIndex}&PageSize=10`);
      state.episodes = data.episodes;
      state.totalCount = data.totalCount;
    };
    const currentPageChange = ()=>{
        doSearch();
    };
	  return {state,doSearch,currentPageChange};
  },
}
</script>
<style>
  em{
    color: red;
  }
</style>