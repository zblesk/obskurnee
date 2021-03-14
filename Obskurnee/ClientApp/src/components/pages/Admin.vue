<template>
<section>
    <h1 id="tableLabel" class="page-title">Admin</h1>
    <div v-if="!isMod" class="main">
        <h2 class="not-mod-text">Je mi líto, sem má přístup pouze moderátor.</h2>
    </div>
    
    <div v-if="isMod" class="main">
    
        <div class="section">
            <h2 class="section-title">Oznámení na hlavní stránce</h2>
            <div class="form-field">
                <label for="notice">Text oznámení</label>
                <textarea v-model="notice" id="notice"></textarea>
            </div>
            <button @click="updateNoticeboard()" class="button-primary">Uložit</button>
        </div>

        <div class="section">
            <h2 class="section-title">Založení nového uživatele</h2>
            <div class="form-field">
                <label for="new-user">E-mailová adresa nového uživatele</label>
                <input type="email" v-model="newUserEmal" id="new-user" />
            </div>
            <button @click="addUser()" class="button-primary">Vytvořit</button>
            <div class="note">
                <div class="note-pic">
                    <img src="../../assets/lamp.svg" alt="lamp icon" />
                </div>
                <p class="note-text">Pro jistotu dej vědět novému čtenáři, ať zkontroluje spam, pokud nenajde registrační email v inboxu.</p>
            </div>
        </div>

        <div class="section">
            <h2 class="section-title">Moderátoři</h2>
            <p>Aktuální moderátoři: <span v-for="mod in mods" v-bind:key="mod.userId"><router-link :to="{ name: 'user', params: { email: mod.email } }">{{mod.name}}</router-link>, </span></p>
            <p>Přidej uživatele k moderátorům:</p>
            <p class="todo">Dodělat styling tabulky, až budu mít založených víc uživatelů</p>
            <table>
                <tr v-for="user in nonMods" v-bind:key="user.userId">
                    <td>{{ user.name }}</td>
                    <td>{{ user.email }}</td>
                    <td><button @click="makeMod(user.email)" class="button-primary">Přidej</button></td>
                </tr>
            </table>

            <p class="todo"><strong>Rozárka: </strong>Budeš přidávat i funkci na odebrání moderátorské funkce?</p>
            <p class="todo-l">Zatim ne</p>
        </div>

        <div class="section">
            <h2 class="section-title">Newsletter</h2>
            <p>Uživatelé přihlášení k odběru newsletteru:</p>
            <p class="todo">dodělat</p>
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
                    this.$notifySuccess(`Aktualizovane`);
                })
            .catch(err => this.$notifyError(err));
            
        },
        addUser()
        {
            axios.post("/api/admin/createuser", { email: this.newUserEmal})
            .then(() => 
                {
                    this.getUsers();
                    this.$notifySuccess(`Pouzivatel vytvoreny`);
                })
            .catch(err => this.$notifyError(err));
        },
        makeMod(email)
        {
            axios.post("/api/admin/makemod/" + email)
            .then(() => 
                {
                    this.$notifySuccess(`${email} povyseny na moderatora`);
                    this.getUsers();
                })
            .catch(err => this.$notifyError(err));
        },
        getNewsletterInfo()
        {
            axios.get("/api/newsletters/all")
            .then(response => this.newsletterInfo = response.data )
            .catch(err => this.$notifyError(err));
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
    .main {
        padding: var(--spacer);
        background-color: var(--c-bckgr-primary);
        max-width: 800px;
        margin: 0 var(--spacer) calc(var(--spacer) * 2) var(--spacer);
    }

    @media screen and (min-width: 840px) {
        .main {
            margin: 0 auto var(--spacer) auto;
        }
    }

    .not-mod-text {
        font-size: 1.2em;
        font-weight: normal;
        text-align: center;
        margin-bottom: 0;
    }

    .not-mod-pic {
        max-width: 400px;
        margin: 0 auto;
    }

    .not-mod img {
        width: 100%;
    }

    .section {
        margin-bottom: calc(2* var(--spacer));
    }

    .section-title {
        margin-top: 0;
        margin-bottom: 0.5em;
    }

    .form-field label {
        display: block;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .form-field textarea {
        width: 100%;
        height: 15em;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .form-field input {
        width: 100%;
        max-width: 380px;
        margin-bottom: calc(var(--spacer) / 2);
    }

    .note {
        display: flex;
        align-items: center;
        margin-top: var(--spacer);
        padding: 0 calc(var(--spacer) / 2);
    }

    .note-pic {
        width: 20px;
        flex-shrink: 0;
    }

    .note-pic img {
        width: 100%;
    }

    .note-text {
        font-size: 0.875em;
        opacity: 0.8;
        margin: 0 0 0 calc(var(--spacer) / 2);
    }

</style>