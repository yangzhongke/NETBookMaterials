<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建专辑</el-button>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="startSort" v-show="!state.isInSortMode">排序</el-button>
    <el-button type="primary" icon="el-icon-success" @click="saveSort" v-show="state.isInSortMode">保存排序</el-button>    
  </el-button-group> 
  <el-table row-key='id'
    :data="state.albums"
    :row-class-name="tableRowClassName"
    style="width: 100%">
    <el-table-column label="排序" v-if="state.isInSortMode" width="120">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="floatItem(state.albums, scope.row)">↑</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="sinkItem(state.albums,scope.row)">↓</button>
        </el-button>         
      </template>     
    </el-table-column>    
    <el-table-column prop="name.chinese" label="中文标题"></el-table-column>
    <el-table-column prop="name.english" label="英文标题"></el-table-column>
    <el-table-column prop="creationTime" label="创建时间"></el-table-column>
    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
        </el-button>
        <el-button type="text" size="small" v-show="scope.row.isVisible">
          <button @click="hideItem(scope.row)">隐藏</button>
        </el-button>
        <el-button type="text" size="small" v-show="!scope.row.isVisible">
          <button @click="showItem(scope.row)">显示</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="edit(scope.row.id)">修改</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="manageChildren(scope.row.id)">管理音频</button>
        </el-button>
      </template>
    </el-table-column>    
  </el-table>    
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
import {floatItem,sinkItem} from '../scripts/ArrayUtils'

export default {
  name: 'AlbumList',
  setup(){
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const categoryId = router.currentRoute.value.query.categoryId;
    const state=reactive({albums:[],categoryId:categoryId,isInSortMode:false});    
    onMounted(async function(){
      const {data}=await axios.get(`${apiRoot}/Listening.Admin/Album/FindByCategoryId/${categoryId}`);      
      state.albums = data;
    });
    const deleteItem=async (album)=>{      
      const id = album.id;
      const name = album.name.chinese;
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.delete(`${apiRoot}/Listening.Admin/Album/DeleteById/${id}`);      
      state.albums = state.albums.filter(e=>e.id!=id);//刷新表格
    };
    const showItem=async (album)=>{      
      const id = album.id;
      await axios.put(`${apiRoot}/Listening.Admin/Album/Show/${id}`);      
      album.isVisible=true;
    };
    const hideItem=async (album)=>{      
      const id = album.id;
      await axios.put(`${apiRoot}/Listening.Admin/Album/Hide/${id}`);      
      album.isVisible=false;
    };
    const tableRowClassName = (scope)=>{
      const row = scope.row;
      return row.isVisible?'visibleAlbum':'inVisibleAlbum';
    };
    const addnew=()=>{
      router.push({name:'AlbumAdd',query:{categoryId:state.categoryId}});
    };
    const edit=(id)=>{
      router.push({name:'AlbumEdit',query:{id:id}});
    };
    const manageChildren=(albumId)=>{
      router.push({name:'EpisodeList',query:{albumId:albumId}});
    };
    const startSort=()=>{
      state.isInSortMode=true;
    };
    const saveSort=async ()=>{
      const ids = state.albums.map(e=>e.id);
      await axios.put(`${apiRoot}/Listening.Admin/Album/Sort/${categoryId}`,{sortedAlbumIds:ids});
      state.isInSortMode = false;
    };
	  return {state,deleteItem,showItem,hideItem,tableRowClassName,addnew,edit,manageChildren,floatItem,sinkItem,startSort,saveSort};
  },
}
</script>
<style lang="css">
  .inVisibleAlbum{text-decoration: line-through;}
  .visibleAlbum{text-decoration:inherit;}
</style>