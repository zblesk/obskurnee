<template>
    <header>
        <div class="top-shelf">
            <div class="books-left"></div>
            <h1 class="logo">Obskurní book club</h1>
            <div class="books-right"></div>
        </div>

        <div class="navigation-wrapper">
            <nav class="navigation">
                <div class="navigation-visible">
                    <div class="navigation-home">
                        <router-link :to="{ name: 'home' }">Domov</router-link>
                    </div>
                    <button type="button" class="navigation-toggler" @click="toggle">
                        <img src="../assets/menu.svg">
                    </button>
                </div>
                <div class="navigation-toggled" v-bind:class="{show: isExpanded}">
                    <div class="navigation-item" v-if="isMod">
                        <router-link :to="{ name: 'admin' }">Admin</router-link>
                    </div>
                    <div class="navigation-item" v-if="isAuthenticated">
                        <router-link :to="{ name: 'booklist' }">Knihy</router-link>
                    </div>
                    <div class="navigation-item" v-if="isAuthenticated">
                        <router-link :to="{ name: 'roundlist' }">Návrhy</router-link>
                    </div>
                    <div class="navigation-item" v-if="isAuthenticated">
                        <router-link :to="{ name: 'recommendationlist' }">Odporúčania</router-link>
                    </div>
                    <div class="navigation-item" v-if="isAuthenticated">
                        <router-link :to="{ name: 'userlist' }">My</router-link>
                    </div>
                    <div class="navigation-login">
                        <login-control></login-control>
                    </div>
                </div>
            </nav>
        </div>
    </header>
</template>


<style scoped>

    /* Header image and logo */

    @media screen and (min-width: 432px) {
        .top-shelf {
            display: flex;
            justify-items: center;
            align-items: flex-end;
        }

        .logo {
            margin-top: 0;
        }
    }

    .books-left,
    .books-right {
        display: none;
        flex-grow: 1;

        height: 50px;
        margin-bottom: -3px;
        background-image: url(../assets/books.svg);
        background-repeat: repeat-x;
        background-origin: content-box;
        background-clip: content-box;
    }

    @media screen and (min-width: 432px) {
        .books-left,
        .books-right {
            display: block;
        }
    }

    .books-left {
        background-position: right bottom;
    }

    .books-right {
        background-position: left bottom;
    }

    .logo {
        margin: 0.5em 0 0 0;
        padding: 0 var(--spacer) 0 var(--spacer);

        font-family: 'Special Elite', cursive;
        font-size: 2em;
        text-align: center;
    }

    @media screen and (min-width: 576px) {
        .books-left,
        .books-right {
            height: 60px;
        }

        .logo {
            font-size: 2.5em;
        }
    }

    @media screen and (min-width: 992px) {
        .books-left,
        .books-right {
            height: 80px;
        }

        .logo {
            font-size: 3.1em;
        }
    }

    /* Navigation */

    .navigation-wrapper {
        background-color: var(--c-bckgr-primary);
        padding-top: calc(var(--spacer) / 2);
        padding-bottom: calc(var(--spacer) / 2);
    }

    .navigation-visible {
        display: flex;
        justify-content: space-between;
    }

    .navigation-toggler {
        width: 40px;
        padding: 10px;
        border: 2px solid var(--c-accent);
        border-radius: 10px;
        outline: none;
        margin-right: var(--spacer);
    }

    .navigation-toggler img {
        width: 100%;
        display: block;
    }

    .navigation-home {
        flex-grow: 1;
        font-weight: bold;
    }

    .navigation-item,
    .navigation-home {
        height: 40px;
        line-height: 40px;
        padding-left: var(--spacer);
    }

    .navigation-item:hover,
    .navigation-home:hover {
        background-color: var(--c-accent);
    }

    .navigation-item:hover a,
    .navigation-home:hover a {
        color: var(--c-font-rev);
    }

    .navigation-item a,
    .navigation-home a {
        font-size: 1.2em;
        color: var(--c-font);
        text-decoration: none;
    }

    .navigation-toggled {
        display: none;
    }

    .navigation-toggled.show {
        display: block;
    }

    @media screen and (min-width: 992px) {
        .navigation-toggler {
            display: none;
        }

        .navigation {
            display: flex;
            justify-content: space-between;
            max-width: 1100px; /* to be reviewed later */
            margin: 0 auto;
        }

        .navigation-visible {
            flex-grow: 1;
            display: block;
        }

        .navigation-home {
            display: inline-block;
            padding-left: calc(var(--spacer) / 2);
            padding-right: calc(var(--spacer) / 2);
        }

        .navigation-toggled,
        .navigation-toggled.show {
            flex-grow: 1;
            display: flex;
            justify-content: flex-end;
        }

        .navigation-item {
            padding-left: calc(var(--spacer) / 2);
            padding-right: calc(var(--spacer) / 2);
        }

        .navigation-login {
            padding-right: var(--spacer);
        }
    }
</style>

<script>
import { mapGetters } from "vuex";
import LoginControl from "./LoginControl.vue";
export default {
    name: "NavMenu",
    components: { LoginControl },
    data() {
        return {
            isExpanded: false
        }
    },
    computed: {
        ...mapGetters("context", ["isAuthenticated", "isMod"])
    },
    methods: {
        collapse() {
            this.isExpanded = false;
        },

        toggle() {
            this.isExpanded = !this.isExpanded;
        }
    }
}
</script>