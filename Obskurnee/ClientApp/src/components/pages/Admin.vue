<template>
<section>
<div v-if="!isMod">Nie si mod ani admin, naco sa sem trepes?!</div>
<div v-if="isMod">
    <h1 id="tableLabel">Admin</h1>
    <div>
        <textarea placeholder="Text noticky" v-model="notice"></textarea>
        <a @click="updateNoticeboard()" class="button">uloz</a>
    </div>
    <div style="margin:auto;">
        <div>
            Novy user: 
            <input v-model="newUserEmal" placeholder="mail" /><button @click="addUser()">Vytvor</button> <br />
            Povedz novemu kamosovi nech pre istotu pozrie aj do spamu, ak neuvidi ziadnu spravu v inboxe.
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
    <div>
        Newsletter subscribers: <br />
        {{ newsletterInfo }}
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
            newsletterInfo: {},
        }
    },
    computed: {
        ...mapState("users", ["users"]),
        ...mapGetters("users", ["nonMods", "mods"]),
        ...mapGetters("context", ["isMod", "isAdmin"]),
    },
    methods: {
        ...mapActions("users", ["getUsers"]),
        getInfo()
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
                    this.getUsers();
                })
            .catch(err => console.log(err));
        },
        makeMod(email)
        {
            axios.post("/api/admin/makemod/" + email)
            .then(() => 
                {
                    console.log("succ ess");
                    this.getUsers();
                })
            .catch(err => console.log(err));
        },
        getNewsletterInfo()
        {
            axios.get("/api/newsletters/all")
            .then(response => this.newsletterInfo = response.data )
            .catch(err => console.log(err));
        }
    },
    mounted() {
        this.getUsers();
        this.getInfo();
        this.getNewsletterInfo();
    }
}
</script>

<style scoped>
table, th, td {
  border: 1px solid darkgreen;
}
</style>