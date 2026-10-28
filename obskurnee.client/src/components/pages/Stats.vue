<template>
    <section>
        <div class="page-tabs">
            <h1 class="page-title tab"
                :class="{ active: activeTab === 'users' }"
                @click="activeTab = 'users'">
                {{ $t('stats.userStats') }}
            </h1>
            <h1 class="page-title tab"
                :class="{ active: activeTab === 'books' }"
                @click="activeTab = 'books'">
                {{ $t('stats.bookStats') }}
            </h1>
        </div>
        <template v-if="activeTab === 'users'">
            <div class="main">
                <h2 class="subsection-title">{{ $t('stats.booksRated') }}</h2>
                <Bar v-if="mostRead" :data="mostRead" :options="chartOptions" />
            </div>
            <div class="main">
                <h2 class="subsection-title">{{ $t('stats.recommendations') }}</h2>
                <Bar v-if="mostRecommended" :data="mostRecommended" :options="chartOptions" />
            </div>
            <div class="main" v-if="mostWins">
                <h2 class="subsection-title">{{ $t('stats.winningBooks') }}</h2>
                <Bar :data="mostWins" :options="chartOptions" />
            </div>
        </template>
        <template v-else>
            <div class="main" v-if="mostReadBooks">
                <h2 class="subsection-title">{{ $t('stats.mostReadBooks') }}</h2>
                <Bar :data="mostReadBooks" :options="bookChartOptions" />
            </div>
            <div class="main" v-if="leastReadBooks">
                <h2 class="subsection-title">{{ $t('stats.leastReadBooks') }}</h2>
                <Bar :data="leastReadBooks" :options="bookChartOptions" />
            </div>
            <div class="main" v-if="bestRatedBooks">
                <h2 class="subsection-title">{{ $t('stats.bestRatedBooks') }}</h2>
                <Bar :data="bestRatedBooks" :options="bookChartOptions" />
            </div>
            <div class="main" v-if="worstRatedBooks">
                <h2 class="subsection-title">{{ $t('stats.worstRatedBooks') }}</h2>
                <Bar :data="worstRatedBooks" :options="bookChartOptions" />
            </div>
        </template>
    </section>
</template>

<script>
import { mapActions, mapState } from "vuex";
import { Bar } from "vue-chartjs";
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from "chart.js";

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export default {
    name: "Stats",
    components: { Bar },
    data() {
        return {
            activeTab: "users",
        };
    },
    watch: {
        '$route.query.tab'() {
            this.syncTabFromRoute();
        },
        activeTab(tab) {
            const current = this.$route.query.tab || 'users';
            if (current !== tab) {
                this.$router.push({ query: { ...this.$route.query, tab } });
            }
        },
    },
    computed: {
        ...mapState("stats", ["userStats", "bookStats"]),
        mostRead() {
            if (!this.userStats || !this.userStats.length) return null;
            const sorted = [...this.userStats].sort((a, b) => b.booksRatedCount - a.booksRatedCount);
            return {
                labels: sorted.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.booksRated'),
                    data: sorted.map(u => u.booksRatedCount),
                    emails: sorted.map(u => u.email),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        mostRecommended() {
            if (!this.userStats || !this.userStats.length) return null;
            const sorted = [...this.userStats].sort((a, b) => b.recommendationCount - a.recommendationCount);
            return {
                labels: sorted.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.recommendations'),
                    data: sorted.map(u => u.recommendationCount),
                    emails: sorted.map(u => u.email),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        mostWins() {
            if (!this.userStats || !this.userStats.length) return null;
            const filtered = [...this.userStats]
                .filter(u => u.winningBooksCount > 0)
                .sort((a, b) => b.winningBooksCount - a.winningBooksCount);
            if (!filtered.length) return null;
            return {
                labels: filtered.map(u => u.userName),
                datasets: [{
                    label: this.$t('stats.winningBooks'),
                    data: filtered.map(u => u.winningBooksCount),
                    emails: filtered.map(u => u.email),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        mostReadBooks() {
            return this.buildBookChart(
                b => b.ratingsCount,
                (a, b) => b.ratingsCount - a.ratingsCount,
                this.$t('stats.ratingsCount'),
                b => b.ratingsCount > 0);
        },
        leastReadBooks() {
            return this.buildBookChart(
                b => b.ratingsCount,
                (a, b) => a.ratingsCount - b.ratingsCount,
                this.$t('stats.ratingsCount'),
                b => b.ratingsCount > 0);
        },
        bestRatedBooks() {
            return this.buildBookChart(
                b => Math.round(b.averageRating * 100) / 100,
                (a, b) => b.averageRating - a.averageRating,
                this.$t('stats.averageRating'),
                b => b.ratingsCount > 0);
        },
        worstRatedBooks() {
            return this.buildBookChart(
                b => Math.round(b.averageRating * 100) / 100,
                (a, b) => a.averageRating - b.averageRating,
                this.$t('stats.averageRating'),
                b => b.ratingsCount > 0);
        },
        accentColor() {
            return getComputedStyle(document.documentElement).getPropertyValue('--c-accent').trim();
        },
        chartOptions() {
            return {
                indexAxis: 'y',
                responsive: true,
                onClick: (event, elements, chart) => {
                    if (!elements.length) return;
                    const { datasetIndex, index } = elements[0];
                    const emails = chart.data.datasets[datasetIndex].emails;
                    this.goToUser(emails && emails[index]);
                },
                onHover: (event, elements) => {
                    event.native.target.style.cursor = elements.length ? 'pointer' : 'default';
                },
                plugins: {
                    legend: { display: false },
                },
                scales: {
                    x: { beginAtZero: true, ticks: { stepSize: 1 } },
                },
            };
        },
        bookChartOptions() {
            return {
                indexAxis: 'y',
                responsive: true,
                onClick: (event, elements, chart) => {
                    if (!elements.length) return;
                    const { datasetIndex, index } = elements[0];
                    const bookIds = chart.data.datasets[datasetIndex].bookIds;
                    this.goToBook(bookIds && bookIds[index]);
                },
                onHover: (event, elements) => {
                    event.native.target.style.cursor = elements.length ? 'pointer' : 'default';
                },
                plugins: {
                    legend: { display: false },
                },
                scales: {
                    x: { beginAtZero: true },
                },
            };
        },
    },
    methods: {
        ...mapActions("stats", ["fetchUserStats"]),
        buildBookChart(valueSelector, comparer, label, filter) {
            if (!this.bookStats || !this.bookStats.length) return null;
            let books = [...this.bookStats];
            if (filter) {
                books = books.filter(filter);
            }
            books = books.sort(comparer).slice(0, 15);
            if (!books.length) return null;
            return {
                labels: books.map(b => b.title),
                datasets: [{
                    label,
                    data: books.map(valueSelector),
                    bookIds: books.map(b => b.bookId),
                    backgroundColor: this.accentColor,
                }],
            };
        },
        goToUser(email) {
            if (email) {
                this.$router.push({ name: 'user', params: { email } });
            }
        },
        goToBook(bookId) {
            if (bookId) {
                this.$router.push({ name: 'book', params: { bookId } });
            }
        },
        syncTabFromRoute() {
            this.activeTab = this.$route.query.tab === 'books' ? 'books' : 'users';
        }
    },
    mounted() {
        this.syncTabFromRoute();
        this.fetchUserStats();
    },
    activated() {
        this.syncTabFromRoute();
        this.fetchUserStats();
    },
}
</script>

<style scoped>
</style>