<template>
<section>
    <h1 id="tableLabel" class="page-title">Seznam bookclubberů</h1>
    
    <div class="user-grid" v-if="users">
        <div class="user-card" v-for="user in users" v-bind:key="user.userId">
            <router-link :to="{ name: 'user', params: { email: user.email } }">
                <div class="user-pic">
                    <img src="../../assets/reader.svg" alt="reader">
                </div>
                <div class="user-desc">
                    <h2 class="user-name">{{ user.name }}</h2>
                    <div class="user-bio" v-html="user.aboutMeHtml"></div>
                </div>
                <div v-for="review in user.currentlyReading" v-bind:key="review.ReviewId">
                    {{ review }} <hr />
                </div>
            </router-link>
        </div>
    </div>

</section>
</template>

<style scoped>
    .user-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: var(--spacer);
        padding: 0 var(--spacer);
    }

    .user-card {
        background-color: var(--c-bckgr-primary);
        padding: var(--spacer);
    }

    .user-pic {
        width: 100px;
        height: 100px;
        margin: 0 auto;
        padding-top: 20px;
        border: 0;
        border-radius: 50%;
        background-color: var(--c-accent);
    }

    .user-pic img {
        max-width: 60%;
        display: block;
        margin: 0 auto;
    }

    .user-name {
        font-size: 1.5em;
        font-weight: bold;
        text-align: center;
        margin-top: var(--spacer);
        margin-bottom: var(--spacer);
    }

    .user-bio {
        font-size: 1em;
    }

    .user-card a {
        text-decoration: none;
    }


</style>


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