<template>
  <input type="text" :value="modelValue" @input="textInput"/>
  <input type="file" ref="fileInput"/>
  <input type="button" value="上传" @click="upload"/>
  <progress max="100" :value="state.completeProgress"></progress>
</template>

<script>
import axios from 'axios';
import { reactive,ref, getCurrentInstance  } from 'vue';
import CryptoJS  from 'crypto-js';

export default
{
  props:{
    modelValue:String,
  },
  setup(props,ctx ){
    const state = reactive({completeProgress:0});
    const fileInput = ref(null);//需要return，需要放到setup()中，而不是upload中。名字需要和ref的名字一样
    const {apiRoot} = getCurrentInstance().proxy;
    const textInput = (event)=>{
      ctx.emit('update:modelValue',event.target.value);
    };
    const beginUpload=(file)=>{
      const formData = new FormData();
      formData.append("file", file);
      const config = 
      {
        headers: { "Content-Type": "multipart/form-data" },
        onUploadProgress: e => 
        {
          var completeProgress = ((e.loaded / e.total * 100) | 0);
          state.completeProgress = completeProgress;
        }
      };
      axios.post(apiRoot+'/FileService/Uploader/Upload', formData, config).then(
      (response) =>
      {
        ctx.emit('update:modelValue',response.data);
      })
      .catch((error)=>
      {
        alert("上传失败"+error);
      });
    };
    const upload=function(){      
      const files = fileInput.value.files;
      if(files.length<=0)
      {
        alert("没有选择文件");
        return;
      }
      const file = files[0];
      let fileReads = new FileReader();
    	//开始读取文件
        fileReads.readAsArrayBuffer(file);
        //读取回调
        fileReads.onload=async function(){
          const wordArray = CryptoJS.lib.WordArray.create(fileReads.result);
          const hash = CryptoJS.SHA256(wordArray).toString();
          const fileSize = file.size;
          //检查是否在服务器端有这个文件
          const {data} = await axios.get(`${apiRoot}/FileService/Uploader/FileExists?fileSize=${fileSize}&sha256Hash=${hash}`);
          if(data.isExists)
          {
            ctx.emit('update:modelValue',data.url);
          }
          else
          {
            //如果没有则上传
            beginUpload(file);
          }
        }
    }
    return {state,upload,fileInput,textInput};
  }
}
</script>

<style scoped>

</style>
