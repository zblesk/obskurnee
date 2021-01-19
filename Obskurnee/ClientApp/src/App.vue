<template>
  <nav-menu></nav-menu>
  <router-view />
</template>

<script>
import { mapActions } from 'vuex'
import NavMenu from './components/NavMenu.vue'

export default {
  name: 'App',
  components: {
      NavMenu
  },
  created () {
    this.restoreContext()
  },
  methods: {
    ...mapActions('context', [
      'restoreContext'
    ])
  }
}
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
