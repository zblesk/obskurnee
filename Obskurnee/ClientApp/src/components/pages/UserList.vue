<template>
<section>
    <h1 id="tableLabel">UserList</h1>
    <div v-for="user in users" v-bind:key="user.userId">
        {{ user }}
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