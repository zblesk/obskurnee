<template>
<section>
    <h1 id="tableLabel" class="page-title">{{$t('menus.admin')}}</h1>
    <div v-if="!isMod" class="main">
        <h2 class="not-mod-text">{{$t('admin.denied')}}</h2>
    </div>
    
    <div v-if="isMod" class="main">
    
        <div class="section">
            <h2 class="section-title">{{$t('admin.noticeboard')}}</h2>
            <div class="form-field">
                <div class="label-md-wrapper">
                    <label for="notice">{{$t('admin.noticeboardContent')}}</label>
                    <div class="mo-md">
                        <div class="mo-md-pic">
                            <img src="../../assets/Markdown-mark.svg" alt="markdown logo">
                        </div>
                        <div class="mo-md-link">
                            <markdown-help-link></markdown-help-link>
                        </div>
                    </div>
                </div>
                <textarea v-model="notice" id="notice" :placeholder="$t('global.markdownSamplePlaceholder')"></textarea>
            </div>
            <button @click="updateNoticeboard()" class="button-primary">{{ $t('menus.save') }}</button>
        </div>

        <div class="section">
            <h2 class="section-title u-mt-sp2">{{ $t('admin.newUserCreation') }}</h2>
            <div class="form-field">
                <label for="new-usermail">{{ $t('admin.newUserEmail') }}*</label>
                <input type="email" v-model="newUserEmail" id="new-usermail" />
            </div>
            <div class="form-field">
                <label for="new-username">{{ $t('admin.newUserName') }}</label>
                <input type="text" v-model="newUserName" id="new-username" />
            </div>
            <button @click="addUser()" class="button-primary">{{ $t('menus.register') }}</button>
            <div class="note u-mt-sp">
                <div class="note-pic">
                    <img src="../../assets/lamp.svg" alt="lamp icon" />
                </div>
                <p class="note-text">{{$t('admin.letUserKnow')}}</p>
            </div>
        </div>

        <div class="section">
            <h2 class="section-title u-mt-sp2">{{$t('admin.mods')}}</h2>
            <p>{{$t('admin.currentMods')}} <span v-for="mod in mods" v-bind:key="mod.userId"><router-link :to="{ name: 'user', params: { email: mod.email } }">{{mod.name}}</router-link>, </span></p>
            <p>{{$t('admin.addMod')}}</p>
            <div class="row" v-for="user in nonMods" v-bind:key="user.userId">
                <div class="wannabe-mod">
                    <router-link :to="{ name: 'user', params: { email: user.email } }">{{ user.name }}</router-link> 
                </div>
                <div class="add-button">
                    <button @click="makeMod(user.email)" class="button-primary button-small">{{$t('menus.add')}}</button>
                </div>
            </div>
        </div>

        <div class="section">
            <h2 class="section-title u-mt-sp2">{{$t('admin.subscribers')}}</h2>
            <p>
                {{$t('admin.basicEvents')}} 
                <span v-for="subscriber in newsletterInfo['basicevents']" v-bind:key="subscriber">
                    <router-link :to="{ name: 'user', params: { email: subscriber.email } }">{{ subscriber.name }}</router-link>, 
                </span>
                <span v-if="!newsletterInfo['basicevents'] || newsletterInfo['basicevents'].length == 0">{{$t('admin.nobody')}}</span>
            </p>
            <p>
                {{$t('admin.allEvents')}} 
                <span v-for="subscriber in newsletterInfo['allevents']" v-bind:key="subscriber">
                    <router-link :to="{ name: 'user', params: { email: subscriber.email } }">{{ subscriber.name }}</router-link>, 
                </span>
                <span v-if="!newsletterInfo['allevents'] || newsletterInfo['allevents'].length == 0">{{$t('admin.nobody')}}</span>
            </p>
        </div>

    </div>
</section>
</template>


<script>
import axios from 'axios';
import { mapActions, mapState, mapGetters } from "vuex";
import MarkdownHelpLink from '../MarkdownHelpLink.vue';
export default {
    name: "Admin",
    components: { MarkdownHelpLink },
    data() {
        return {
            newUserEmail: "",
            newUserName: "",
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
                    this.$notifySuccess(this.$t('messages.updated'));
                })
            .catch(this.$handleApiError);
            
        },
        addUser()
        {
            axios.post("/api/admin/createuser", { email: this.newUserEmail, name: this.newUserName})
            .then(() => 
                {
                    this.getUsers();
                    this.$notifySuccess(this.$t('messages.userCreated'));
                })
            .catch(this.$handleApiError);
        },
        makeMod(email)
        {
            axios.post("/api/admin/makemod/" + email)
            .then(() => 
                {
                    this.$notifySuccess(`${email} povyseny na moderatora`);
                    this.getUsers();
                })
            .catch(this.$handleApiError);
        },
        getNewsletterInfo()
        {
            axios.get("/api/newsletters/all")
            .then(response => this.newsletterInfo = response.data )
            .catch(this.$handleApiError);
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

    .section:not(:last-child) {
        margin-bottom: calc(2* var(--spacer));
    }

    .section-title {
        margin-bottom: var(--spacer);
    }

    .form-field input {
        max-width: 380px;
    }

    .button-small {
        padding: 0.3em 0.6em;
    }

    .row {
        margin-top: 0;
        margin-bottom: 0;
        border-top: 1px solid var(--c-accent);
    }

    .row:last-child {
        border-bottom: 1px solid var(--c-accent);
    }

    .wannabe-mod {
        text-align: center;
        padding: calc(var(--spacer) / 2);
    }

    .add-button {
        text-align: center;
        padding: 0 calc(var(--spacer) / 2) calc(var(--spacer) / 2) calc(var(--spacer) / 2);
    }

    @media screen and (min-width: 576px) {
        .row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            max-width: 400px;
        }

        .add-button {
            padding-top: calc(var(--spacer) / 2);
        }
    }

</style>