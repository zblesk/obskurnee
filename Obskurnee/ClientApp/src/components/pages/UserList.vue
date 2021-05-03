<template>
<section>
    <h1 id="tableLabel" class="page-title">{{$t('user.list')}}</h1>
    
    <div class="user-grid" v-if="users">
        <div class="user-card" v-for="user in users" v-bind:key="user.userId">
            <router-link :to="{ name: 'user', params: { email: user.email } }">
                <div v-if="user.avatarUrl" class="user-pic">
                    <img :src="user.avatarUrl" :title="user.name" :alt="user.name" />
                    <p class="mod-text" v-if="user.isModerator">{{$t('global.mod')}}</p>
                </div>
                <div v-else class="user-pic-placeholder">
                    <img src="../../assets/reader.svg" :title="user.name" :alt="user.name" />
                    <p class="mod-text" v-if="user.isModerator">{{$t('global.mod')}}</p>
                </div>
            </router-link>
            <div class="user-desc">
                <router-link :to="{ name: 'user', params: { email: user.email } }">
                    <h2 class="user-name">{{ user.name }}</h2>
                </router-link>
                <div class="user-bio" v-html="user.aboutMeHtml"></div>
            </div>

            <div class="reading" v-if="userHasCurrentlyReading(user.userId)" >
                <h3 class="reading-title">{{$t('user.currentlyReading')}}</h3>
                <ul class="reading-list">
                    <li v-for="review in usersCurrentlyReading(user.userId)" v-bind:key="review.ReviewId"><a :href="review.reviewUrl">{{ review.author }}: <strong>{{ review.bookTitle }}</strong></a></li>
                </ul>
            </div>
            <div  class="reading" v-else>
                <h3 class="reading-title">{{$t('user.readingNothing')}}</h3>
            </div>            
        </div>
    </div>

</section>
</template>

<style scoped>

    .user-card {
        background-color: var(--c-bckgr-primary);
        padding: var(--spacer);
        margin: 0 var(--spacer) var(--spacer) var(--spacer);
    }

    .user-pic {
        width: 100px;
        height: 100px;
        margin: 0 auto;
        border: 0;
        border-radius: 50%;
        position: relative;
    }

    .user-pic img {
        width: 100%;
        border-radius: 50%;
    }

    .user-pic-placeholder {
        width: 100px;
        height: 100px;
        margin: 0 auto;
        padding-top: 20px;
        border: 0;
        border-radius: 50%;
        background-color: var(--c-accent);
        position: relative;
    }

    .user-pic-placeholder img {
        max-width: 60%;
        display: block;
        margin: 0 auto;
    }

    .mod-text {
        color: var(--c-accent);
        font-variant: small-caps;
        font-size: 1.25rem;
        font-weight: bold;
        transform: rotate(40deg);
        position: absolute;
        top: -10px;
        right: -10px;
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
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "UserList",
    data() {
        return {
        }
    },
    computed: {
        ...mapState("users", ["users"]),
        ...mapGetters("reviews", ["usersCurrentlyReading", "userHasCurrentlyReading"]),
    },
    methods: {
        ...mapActions("users", ["getUsers"]),
        ...mapActions("reviews", ["fetchCurrentlyReading"]),
    },
    mounted() {
        this.getUsers();
        this.fetchCurrentlyReading();
    }
}
</script>