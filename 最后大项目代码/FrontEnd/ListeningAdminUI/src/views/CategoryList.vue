<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addNew">创建分类</el-button>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="startSort" v-show="!state.isInSortMode">排序</el-button>
    <el-button type="primary" icon="el-icon-success" @click="saveSort" v-show="state.isInSortMode">保存排序</el-button>
  </el-button-group>
  <el-table row-key='id' ref="table" :data="state.categories" style="width: 100%">
    <el-table-column label="排序" v-if="state.isInSortMode" width="120">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="floatItem(state.categories, scope.row)">↑</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="sinkItem(state.categories,scope.row)">↓</button>
        </el-button>         
      </template>     
    </el-table-column>
    <el-table-column prop="name.chinese" label="中文标题"  width="150px"></el-table-column>
    <el-table-column prop="name.english" label="英文标题" width="120px"></el-table-column>
    <el-table-column prop="creationTime" label="创建时间" width="180px"></el-table-column>
    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="edit(scope.row.id)">修改</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="manageChildren(scope.row.id)">管理专辑</button>
        </el-button>
      </template>
    </el-table-column>    
  </el-table>  
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;
import {useRouter } from 'vue-router';
import {floatItem,sinkItem} from '../scripts/ArrayUtils'

export default {
  name: 'CategoryList',
  setup(){
    //axios.defaults.headers.Authorization  = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNkNGIwY2U1LTIwZDgtNDkxYS04YjIxLWQ2N2M3NjY5YjY1OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiZXhwIjoxNjYzMzE4Mzc1LCJpc3MiOiJteUlzc3VlciIsImF1ZCI6Im15QXVkaWVuY2UifQ.VECL0niSsS2zpVFGYmwaoTRgYedbCig1cwbglb0Gbps";

    const state=reactive({categories:[],isInSortMode:false});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Listening.Admin/Category/FindAll`);
      state.categories = data;
    });
    const deleteItem=async (category)=>{      
      const id = category.id;
      const name = category.name.chinese;
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.delete(`${apiRoot}/Listening.Admin/Category/DeleteById/${id}`);      
      state.categories = state.categories.filter(e=>e.id!=id);//刷新表格
    };
    const addNew =()=>{
      router.push({name: 'CategoryAdd'});
    };
    const edit =(id)=>{
      router.push({name: 'CategoryEdit',query:{id:id}});
    };
    const manageChildren =(id)=>{
      router.push({name: 'AlbumList',query:{categoryId:id}});
    };
    const startSort=()=>{
      state.isInSortMode=true;
    };
    const saveSort=async ()=>{
      const ids = state.categories.map(e=>e.id);
      await axios.put(`${apiRoot}/Listening.Admin/Category/Sort`,{sortedCategoryIds:ids});
      state.isInSortMode = false;
    };
	  return {state,deleteItem,addNew,edit,manageChildren,floatItem,sinkItem,startSort,saveSort};
  },
}
</script>
<style scoped>
</style>