<template>
<section>
    <h1 id="tableLabel" class="page-title u-mb">Seznam bookclubberů</h1>
    <h2 class="page-note">(moderátoři mají magické schopnosti)</h2>
    
    <div class="user-grid" v-if="users">
        <div class="user-card" v-for="user in users" v-bind:key="user.userId">
            <router-link :to="{ name: 'user', params: { email: user.email } }">
                <div class="user-pic">
                    <img v-if="user.isModerator" src="../../assets/fairy-white.svg" alt="fairy icon" class="user-pic-mod">
                    <img v-else src="../../assets/reader.svg" alt="reader">
                </div>
            </router-link>
            <div class="user-desc">
                <router-link :to="{ name: 'user', params: { email: user.email } }">
                    <h2 class="user-name">{{ user.name }}</h2>
                </router-link>
                <div class="user-bio" v-html="user.aboutMeHtml"></div>
            </div>
            <div class="reading" v-if="user.currentlyReading.length">
                <h3 class="reading-title">Právě čte:</h3>
                <ul class="reading-list">
                    <li v-for="review in user.currentlyReading" v-bind:key="review.ReviewId"><a :href="review.reviewUrl">{{ review.author }}: <span class="reading-book">{{ review.bookTitle }}</span></a></li>
                </ul>
            </div>
            <div  class="reading" v-else>
                <h3 class="reading-title">Práve nič nečíta.</h3>
            </div>            
        </div>
    </div>

</section>
</template>

<style scoped>

    .u-mb {
        margin-bottom: calc(var(--spacer) / 2);
    }

    .page-note {
        text-align: center;
        font-weight: normal;
        margin-bottom: var(--spacer);
    }

    .user-card {
        background-color: var(--c-bckgr-primary);
        padding: var(--spacer);
        margin: 0 var(--spacer) var(--spacer) var(--spacer);
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

    .user-pic-mod {
        position: relative;
        left: 8px;
    }

    .user-name {
        color: var(--c-primary);
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

    .reading-title {
        margin-top: var(--spacer);
        margin-bottom: var(--spacer);
    }

    .reading-book {
        font-weight: bold;
    }

    .reading-list li {
        line-height: 1.5;
        margin-bottom: var(--spacer);
    }

    .reading-list li:last-child {
        margin-bottom: 0;
    }

    @media screen and (min-width: 400px) {
        .user-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
            gap: var(--spacer);
            padding: 0 var(--spacer);
            margin-bottom: var(--spacer);
        }

        .user-card {
            margin: 0 0 0 0;
        }
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