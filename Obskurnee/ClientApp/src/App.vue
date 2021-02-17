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


html {
  -webkit-box-sizing: border-box;
          box-sizing: border-box;
  }

*, :after, :before {
  -webkit-box-sizing: inherit;
          box-sizing: inherit;
  margin: 0;
  padding: 0;
}

.todo {
  margin: 2em;
  color: orangered;
  text-align: center;
}

#app {
  font-family: 'Raleway', Helvetica, Arial, sans-serif;
  font-size: 100%;
  color: #292929;

  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin-top: 60px;
}

.page-title {
  font-size: 2.5em;
  text-align: center;
  padding-top: 1em;
  margin-bottom: 0.8em;
}

.button__primary {
  border: 0;
  border-radius: 4px;
  background-color: #79AA2D;
  color: white;
  font-size: 16px;
  padding: 10px 20px;
  cursor: pointer;
}

.button__primary:hover {
  color: white;
  text-decoration: none;
  background-color: #648d25;
}

</style>
