<template>
<section>
<h1 id="tableLabel" class="page-title">Kolá návrhov</h1>
<button v-if="isMod && hide" class="button-primary new-round" @click="toggleVisibility">Založit nové kolo</button>

<div v-if="isMod && !hide" class="main">
    <h2 class="form-heading">Založení nového kola</h2>
    <div class="form-field form-flex">
        <span>Začít s </span>
        <div class="form-field-radios">
            <input type="radio" id="Books" value="Books" v-model="newRound.topic">
            <label for="Books" class="label-radio">knihami</label>
        </div>
        <span class="radio-between">nebo</span>
        <div class="form-field-radios">
            <input type="radio" id="Themes" value="Themes" v-model="newRound.topic">
            <label for="Themes" class="label-radio">tématy</label>
        </div>
    </div>
    <div class="form-field">
        <label for="name">Názov (napr. 'Kniha IXY')</label>
        <input v-model="newRound.title" id="name" required />
    </div>
    <div class="form-field">
        <label for="description">Popis</label>
        <textarea v-model="newRound.description" id="description"></textarea>
    </div>
    <div class="buttons">
        <button @click="cNewRound" class="button-primary">Nové kolo</button>
        <button class="button-secondary hide-form" @click="toggleVisibility">Schovat formulář</button>
    </div>
</div>

<p v-if="!rounds" class="alert-inline">Cakaj, nacitavam</p>
<div v-for="round of rounds" v-bind:key="round" class="round">
    <div class="round-inner">
        <h2 class="round-title">{{ round.title }}</h2>
        <div class="round-mo">
            <div class="mo-wrapper">
                <div class="mo mo-theme" v-if="round.themeDiscussionId">
                    <div class="mo-icon">
                        <img src="../../assets/book-shelves.svg" alt="book shelves icon">
                    </div>
                    <div class="mo-text">
                        <p class="mo-link">
                            <router-link v-if="round.themeDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.themeDiscussionId } }">Návrhy tém</router-link>
                        </p>
                        <p class="mo-link">
                            <router-link v-if="round.themePollId" :to="{ name: 'poll', params: { pollId: round.themePollId } }">Hlasovanie o témach</router-link>
                        </p>
                    </div>
                </div>
                <div class="mo" v-if="round.bookDiscussionId">
                    <div class="mo-icon">
                        <img src="../../assets/magic-book.svg" alt="magic book icon">
                    </div>
                    <div class="mo-text">
                        <p class="mo-link">
                            <router-link v-if="round.bookDiscussionId" :to="{ name: 'discussion', params: { discussionId: round.bookDiscussionId } }">Návrhy kníh</router-link>
                        </p>
                        <p class="mo-link">
                            <router-link v-if="round.bookPollId" :to="{ name: 'poll', params: { pollId: round.bookPollId } }">Hlasovanie o knihách</router-link>
                        </p>
                    </div>
                </div>
            </div>
            <div class="winner" v-if="round.bookId">
                <div class="winner-cover">
                    <router-link v-if="round.bookId" :to="{ name: 'book', params: { bookId: round.bookId } }">
                        <img v-if="round.book?.post?.imageUrl"
                            :src="round.book?.post?.imageUrl" :alt="round.book.post.title" :title="round.book.post.title">
                        <img v-else 
                            src="../../assets/book.svg" :alt="round.book?.post?.title" :title="round.book?.post?.title">
                    </router-link>
                </div>
                <div class="winner-desc">
                    <p class="winner-book"><router-link v-if="round.bookId" :to="{ name: 'book', params: { bookId: round.bookId } }">{{ round.book?.post?.title }}</router-link></p>
                    <p class="winner-author">{{ round.book?.post?.author }}</p>
                </div>
            </div>
        </div>
    </div>
</div>
</section>
</template>


<script>
import { mapActions, mapState, mapGetters } from "vuex";
export default {
    name: "RoundList",
    data() { 
        return {
            newRound: {
                topic: "Books",
            },
            hide: true,
        }
    },
    computed: {
        ...mapState("rounds", ["rounds"]),
        ...mapGetters("context", ["isMod"])
    },
    methods: {
        ...mapActions("rounds", ["fetchRounds", "createNewRound"]),
        cNewRound()
        {
            this.createNewRound(this.newRound).then(() => 
            {
                this.newRound = {};
                this.hide = true;
            });
        },
        toggleVisibility()
        {
            this.hide = !this.hide;
        }
    },
    mounted() {
        this.fetchRounds();
    }
}
</script>

<style scoped>
    /* Form */
    .new-round {
        display: block;
        margin: 0 auto;
    }

    .hide-form {
        margin-top: var(--spacer);
    }

    @media screen and (min-width: 576px) {
        .hide-form {
            margin-top: 0;
            margin-left: var(--spacer);
        }
    }


    .form-heading {
        font-size: 1.5em;
        padding: 0;
        margin-top: 0;
        margin-bottom: var(--spacer);
    }

    .label-radio {
        display: inline-block;
    }

    .form-flex {
        display: flex;
        flex-direction: column;
    }

    .form-field input[type="radio"] {
        display: inline-block;
        width: auto;
        margin-left: 0;
        margin-right: calc(var(--spacer) / 4);
    }

    .radio-between {
        margin-left: calc(var(--spacer) / 2);
        margin-right: calc(var(--spacer) / 4); 
    }

    .form-field-radios {
        margin-top: calc(var(--spacer) / 2);
        margin-bottom: calc(var(--spacer) / 2);
    }

    @media screen and (min-width: 450px) {
        .form-flex {
            flex-direction: row;
        }

        .form-field input[type="radio"] {
            margin-left: calc(var(--spacer) / 2);
        }

        .form-field-radios {
            margin-top: 0;
            margin-bottom: 0;
        }
    }

    .form-field textarea {
        height: 5em;
    }

    /* List of rounds */

    /* fonts */

    .round-title {
        margin-top: 0;
        margin-bottom: calc(var(--spacer) * 2);
    }

    .mo-link a {
        color: var(c-accent);
    }

    .winner-book,
    .winner-author {
        text-align: center;
    }

    .winner-book {
        font-weight: bold;
    }

    .winner-book a {
        text-decoration: none;
    }

    /* basic layout */
    .round {
        background-color: var(--c-bckgr-primary);
        padding: var(--spacer);
        margin-top: calc(var(--spacer) * 2);
        margin-bottom: calc(var(--spacer) * 2);
    }

    .round-inner {
        max-width: 1200px; /* to change later */
        margin: 0 auto;
    }

    /* pictures */
    .mo-icon {
        width: 80px;
        height: auto;
    }

    .mo-icon img {
        width: 100%;
    }

    .winner-cover {
        height: 80px;
    }

    .winner-cover img {
        height: 100%;
    }

    /* round layout */
    .round-title {
        text-align: center;
    }

    .mo {
        text-align: center;
        margin-bottom: calc(var(--spacer) * 2);
    }

    .mo-icon {
        margin: 0 auto calc(var(--spacer) / 2) auto;
    }

    .mo-link {
        margin: 0;
    }

    .mo-link:first-child {
        margin-bottom: var(--spacer);
    }

    .winner {
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    .winner-cover {
        margin-bottom: var(--spacer);
    }

    .winner-book {
        margin-bottom: calc(var(--spacer) / 2);
    }

    .winner-author {
        margin-bottom: 0;
    }

    @media screen and (min-width: 576px) {
        .mo {
            display: flex;
            justify-items: flex-start;

            text-align: left;
            align-items: center;
            margin-bottom: 0;
            min-width: 255px;
        }

        .mo-theme {
            margin-bottom: var(--spacer);
        }

        .mo-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .mo-icon {
            margin: 0 var(--spacer) 0 0;
        }

        .mo-link:first-child {
            margin-bottom: 0;
        }

        .mo-text {
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            min-height: 55px;
        }

        .round-mo {
            display: flex;
            justify-content: center;
        }

        .winner {
            margin: 0 0 0 calc(var(--spacer) * 2);
            flex-direction: row;
        }

        .winner-cover {
            margin: 0 var(--spacer) 0 0;
            flex-shrink: 0;
        }

        .winner-book,
        .winner-author {
            text-align: left;
        }

        .winner-desc {
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            min-height: 55px;
        }
    }

    @media screen and (min-width: 900px) {
        .mo-wrapper {
            flex-direction: row;
            justify-content: space-between;
        }

        .mo-theme {
            margin-right: calc(var(--spacer) * 2);
            margin-bottom: 0;
        }
    }

</style>