<template>
<section>
<div v-if="!isMod">Nie si mod ani admin, naco sa sem trepes?!</div>
<div v-if="isMod">
    <h1 id="tableLabel">Admin</h1>
    <div>
        <textarea placeholder="Text noticky" v-model="notice"></textarea>
        <a @click="updateNoticeboard()" class="button">uloz</a>
    </div>
    <p class="todo">mailer</p>
    <div style="margin:auto;">
        <div>
            Novy user: 
            <input v-model="newUserEmal" placeholder="mail" /><button @click="addUser()">Vytvor</button>
        </div>
        <div>
            Moderatori: <span v-for="mod in mods" v-bind:key="mod.userId"><router-link :to="{ name: 'user', params: { email: mod.email } }">{{mod.name}}</router-link>, </span>
        </div>
        <div>
            Urob moderatora:
            <table>
                <tr v-for="user in nonMods" v-bind:key="user.userId">
                    <td>{{ user.name }}</td>
                    <td>{{user.email}}</td>
                    <td><a @click="makeMod(user.email)" class="button">Urob moda</a></td>
                </tr>
            </table>
        </div>
    </div>
</div>
</section>
</template>


<script>
import axios from 'axios';
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "Admin",
    data() {
        return {
            newUserEmal: "",
            notice: "",
        }
    },
    computed: {
        ...mapState("users", ["users"]),
        ...mapGetters("users", ["nonMods", "mods"]),
        ...mapGetters("context", ["isMod", "isAdmin"]),
    },
    methods: {
        ...mapActions("users", ["getUsers"]),
        loadInfo()
        {
            axios.get("/api/admin")
                .then(response => 
                {
                    this.notice = response.data.noticeboard;
                })
        },
        updateNoticeboard()
        {
            axios.post("/api/admin/noticeboard", { text: this.notice })
            .then(() => 
                {
                    console.log("updated");
                })
            .catch(err => console.log(err));
            
        },
        addUser()
        {
            axios.post("/api/admin/createuser", { email: this.newUserEmal})
            .then(() => 
                {
                    console.log("succ ess");
                    this.getUsers();
                })
            .catch(err => console.log(err));
        },
        makeMod(email)
        {
            console.log(email);
            axios.get("/api/admin/makemod/" + email)
            .then(() => 
                {
                    console.log("succ ess");
                    this.getUsers();
                })
            .catch(err => console.log(err));
        }
    },
    mounted() {
        this.getUsers();
        this.loadInfo();
    }
}
</script>

<style scoped>
table, th, td {
  border: 1px solid darkgreen;
}
</style>