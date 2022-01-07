<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建音频</el-button>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="startSort" v-show="!state.isInSortMode">排序</el-button>
    <el-button type="primary" icon="el-icon-success" @click="saveSort" v-show="state.isInSortMode">保存排序</el-button>    
  </el-button-group> 
  <el-table row-key='id' :data="state.encodingEpisodes"  v-if="state.encodingEpisodes.length>0" style="width: 100%">
    <el-table-column prop="name.chinese" label="中文标题" width="200px"></el-table-column>
    <el-table-column prop="name.english" label="英文标题" width="150px"></el-table-column>
    <el-table-column prop="durationInSecond" width="60px" label="秒数"> 
    </el-table-column>     
    <el-table-column label="转码状态" width="120px">
       <template #default="scope">
        {{renderEncodingStatus(scope.row.status)}}
      </template>       
    </el-table-column>    
  </el-table>

  <el-table row-key='id' :data="state.episodes" :row-class-name="tableRowClassName" style="width: 100%">
    <el-table-column label="排序" v-if="state.isInSortMode" width="120">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="floatItem(state.episodes, scope.row)">↑</button>
        </el-button>
        <el-button type="text" size="small">
          <button @click="sinkItem(state.episodes,scope.row)">↓</button>
        </el-button>         
      </template>     
    </el-table-column>    
    <el-table-column prop="name.chinese" label="中文标题"></el-table-column>
    <el-table-column prop="name.english" label="英文标题"></el-table-column>
    <el-table-column prop="creationTime" label="创建时间"></el-table-column>
    <el-table-column label="秒数">
		<template #default="scope">
		  <span>{{parseInt(scope.row.durationInSecond)}}</span>
		</template>        
    </el-table-column>     
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
      </template>
    </el-table-column>    
  </el-table>      
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
import {floatItem,sinkItem} from '../scripts/ArrayUtils';
import * as signalR from '@microsoft/signalr';

export default {
  name: 'EpisodeList',
  setup(){
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const albumId = router.currentRoute.value.query.albumId;
    const state=reactive({episodes:[],encodingEpisodes:[],albumId:albumId,isInSortMode:false});    
    onMounted(async function(){
      await reloadData();
      //禁用Negotiation，客户端一直连接初始的服务器，这样服务器搞负载均衡（不用Redis BackPlane等）也没问题
      const options = {
        skipNegotiation: true,
        transport: 1 // 强制WebSockets
      };
      const hub = new signalR.HubConnectionBuilder()
      .withUrl(`${apiRoot}/Listening.Admin/Hubs/EpisodeEncodingStatusHub`,options)
      .build();
      hub.start();
      hub.on('OnMediaEncodingStarted',id=>{        
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Started";
      });
      hub.on('OnMediaEncodingFailed',id=>{
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Failed";
      });
      hub.on('OnMediaEncodingCompleted',id=>{
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Completed";
        reloadData();//遇到由完成任务的就刷新数据
      });
    });
    const reloadData=async()=>{
      let resp = await axios.get(`${apiRoot}/Listening.Admin/Episode/FindByAlbumId/${albumId}`);
      state.episodes = resp.data;      
      resp = await axios.get(`${apiRoot}/Listening.Admin/Episode/FindEncodingEpisodesByAlbumId/${albumId}`);
      state.encodingEpisodes = resp.data;
    };
    const deleteItem=async (episode)=>{      
      const id = episode.id;
      const name = episode.name.chinese;
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.delete(`${apiRoot}/Listening.Admin/Episode/DeleteById/${id}`);      
      state.episodes = state.episodes.filter(e=>e.id!=id);//刷新表格
    };
    const showItem=async (episode)=>{      
      const id = episode.id;
      await axios.put(`${apiRoot}/Listening.Admin/Episode/Show/${id}`);      
      episode.isVisible=true;
    };
    const hideItem=async (episode)=>{      
      const id = episode.id;
      await axios.put(`${apiRoot}/Listening.Admin/Episode/Hide/${id}`);      
      episode.isVisible=false;
    };
    const tableRowClassName = (scope)=>{
      const row = scope.row;
      return row.isVisible?'visibleEpisode':'inVisibleEpisode';
    };   
    const addnew=()=>{
      router.push({name:'EpisodeAdd',query:{albumId:state.albumId}});
    }; 
    const edit=(id)=>{
      router.push({name:'EpisodeEdit',query:{id:id}});
    };
    const startSort=()=>{
      state.isInSortMode=true;
    };
    const saveSort=async ()=>{
      const ids = state.episodes.map(e=>e.id);
      await axios.put(`${apiRoot}/Listening.Admin/Episode/Sort/${albumId}`,{sortedEpisodeIds:ids});
      state.isInSortMode = false;
    };    
    const renderEncodingStatus = (status)=>{
      const dict = {"Created":"等待转码","Started":"转码中","Failed":"转码失败","Completed":"转码完成"};
      const value = dict[status];
      return value?value:"未知";
    };
	  return {state,deleteItem,showItem,hideItem,tableRowClassName,addnew,edit,
      floatItem,sinkItem,startSort,saveSort, renderEncodingStatus,reloadData};
  },
}
</script>
<style lang="css">
  .inVisibleEpisode{text-decoration: line-through;}
  .visibleEpisode{text-decoration:inherit;}
</style>