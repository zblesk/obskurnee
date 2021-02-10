<template>
<section>
    <h1 id="tableLabel">UserList</h1>
    <p class="todo">Karticky so zakladnym info?</p>
    
    <div v-for="user in users" v-bind:key="user.userId">
      <router-link :to="{ name: 'user', params: { email: user.email } }"> {{ user }} </router-link>
    </div>
</section>
</template>


<script>
import { mapActions, mapState } from "vuex";
export default {
    name: "UserList",
    data() {
        return {
        }
    },
    computed: {
        ...mapState("users", ["users"]),
    },
    methods: {
        ...mapActions("users", ["getUsers"]),
    },
    mounted() {
        this.getUsers();
    }
}
</script>