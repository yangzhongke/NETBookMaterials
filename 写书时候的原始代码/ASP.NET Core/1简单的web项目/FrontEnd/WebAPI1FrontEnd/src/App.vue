<template>
  <img alt="Vue logo" src="./assets/logo.png" />
  <div><router-link to="Test">Test</router-link></div>
  <div><router-link to="Login">Login</router-link>
  </div>
  <router-view />  
</template>

<script setup>

</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
